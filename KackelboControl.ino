#include <OneWire.h>
#include <DallasTemperature.h>
#include <ArduinoJson.h>
#include <ArduinoJson.hpp>
#include "arduino_secrets.h"
#include "WiFiS3.h"
#include "WiFiSSLClient.h"
#include "IPAddress.h"
#include <ArduinoHttpClient.h>
#include "RTC.h"

//Wifi and api keys
char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;
char apiKey[] = SECRET_API_KEY;

//Wifi and http client
int HTTPS_PORT = 443;
String HTTPS_GET = "GET";
String HTTPS_POST = "POST";
char HOST_NAME[] = "kackelbocontrolapi.azurewebsites.net";  // hostname of web server:

int status = WL_IDLE_STATUS;
WiFiSSLClient wifiClient;
HttpClient httpClient(wifiClient, HOST_NAME, HTTPS_PORT);

//variables to use to check if relays should trigger
int lightOnHour;
int lightOnMinute;
int lightOffHour;
int lightOffMinute;
float minTemp;
float maxTemp;
bool useSunLight;
int sunUpHour;
int sunUpMinute;
int sunDownHour;
int sunDownMinute;

//Temprature readers
const int TEMP_INDOORS = 12;
OneWire oneWireIn(TEMP_INDOORS);
DallasTemperature tempSensorIn(&oneWireIn);
float tempIndoorCelsius;

const int TEMP_OUTDOORS = 13;
OneWire oneWireOut(TEMP_OUTDOORS);
DallasTemperature tempSensorOut(&oneWireOut);
float tempOutdoorCelsius;

//Sensor variables
int lastSentSensorValues;
const int RELAY_PIN_LAMP = 4;
const int RELAY_PIN_HEAT = 5;
bool lampOn = false;
bool heatOn = false;

//Sync variables
unsigned long previousTriggerMillis = 0;
const long syncTriggersInterval = 60000;
unsigned long previousTimeSyncMillis = 0;
const long timeSyncInterval = 3600000;

void setup() {
  Serial.begin(115200);
  pinMode(RELAY_PIN_LAMP, OUTPUT);
  pinMode(RELAY_PIN_HEAT, OUTPUT);
  tempSensorIn.begin();
  tempSensorOut.begin();

  SetUpWifi();
  RTC.begin();

  TimeSync();
  GetTriggerInformation();  
}



void loop() {

  unsigned long currentMillis = millis();
  if (currentMillis - previousTriggerMillis >= syncTriggersInterval) {
    GetTriggerInformation();
    previousTriggerMillis = currentMillis;
  }

  currentMillis = millis();
  if (currentMillis - previousTimeSyncMillis >= timeSyncInterval) {
    TimeSync();
    previousTimeSyncMillis = currentMillis;
  }

  PostSensorValues();
  CheckHeat();
  CheckLight();
}

//Setting up wifi
void SetUpWifi() {
  while (!Serial) {
    ;
  }

  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    while (true)
      ;
  }

  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Upgrade firmware.");
  }

  while (status != WL_CONNECTED) {
    Serial.print("Trying to connect to network with SSID: ");
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);
    Serial.println("Checking status....");
    delay(10000);

    Serial.println("Connected to network:");
    Serial.println(ssid);

    long rssi = WiFi.RSSI();
    Serial.println("signal strength (RSSI):");
    Serial.print(rssi);
    Serial.println(" dBm");
  }
}


#pragma region SensorTriggers

#pragma region Heater
void CheckHeat() {
  tempSensorIn.requestTemperatures();
  tempIndoorCelsius = tempSensorIn.getTempCByIndex(0);

  //if cold and heater is off, turn on heat.
  if (tempIndoorCelsius < minTemp) {
    TurnOnHeat();
  }
  //if hot and heater is on, turn off heat.
  else if (tempIndoorCelsius > maxTemp) {
    TurnOffHeat();
  }
}

void TurnOnHeat() {
  if (heatOn) {
    return;
  }
  Serial.println("Turning on heater!");
  digitalWrite(RELAY_PIN_HEAT, HIGH);
  PostHeatSwitchChange(true);
}

void TurnOffHeat() {
  if (!heatOn) {
    return;
  }
  Serial.println("Turning off heater!");
  digitalWrite(RELAY_PIN_HEAT, LOW);
  PostHeatSwitchChange(false);
}

#pragma endregion

#pragma region Light

void CheckLight() {
  RTCTime currentTime;
  RTC.getTime(currentTime);

  int currentHour = currentTime.getHour();
  int currentMinute = currentTime.getMinutes();

  //make ints from time value to be able to compare
  int intCurrentTime = currentHour * 100 + currentMinute;
  int lightOnTime = lightOnHour * 100 + lightOnMinute;
  int lightOffTime = lightOffHour * 100 + lightOffMinute;

  if (!useSunLight) {

    //if current time is after lightOnTime and before lightOffTime
    if (lightOnTime <= intCurrentTime && intCurrentTime < lightOffTime) {

      TurnOnLamp();

    } else {

      //if not in timespan, turn off lamp.
      TurnOffLamp();
    }
  } else if (useSunLight) {
    int sunRiseTime = sunUpHour * 100 + sunUpMinute;
    int sunDownTime = sunDownHour * 100 + sunDownMinute;

    //if lightOnTime is before current time and current time is before sunrise.
    //if sundown is before current time and current time is before lightOffTime
    if ((lightOnTime <= intCurrentTime && intCurrentTime < sunRiseTime) || (sunDownTime <= intCurrentTime && intCurrentTime < lightOffTime)) {
      TurnOnLamp();
    } else {
      TurnOffLamp();
    }
  }
}

void TurnOnLamp() {
  if (lampOn) {
    return;
  }
  Serial.println("Lamp on!");
  digitalWrite(RELAY_PIN_LAMP, HIGH);
  PostLightSwitchChange(true);
}

void TurnOffLamp() {
  if (!lampOn) {
    return;
  }
  Serial.println("Lamp off!");
  digitalWrite(RELAY_PIN_LAMP, LOW);
  PostLightSwitchChange(false);
}
#pragma endregion

#pragma endregion

#pragma region GET
void TimeSync() {

  char getTriggerPath[] = "/Arduino/timeSync";
  Serial.println("Syncing time");

  httpClient.beginRequest();
  httpClient.get(getTriggerPath);
  httpClient.sendHeader("ApiKey", SECRET_API_KEY);
  httpClient.endRequest();

  int statusCode = httpClient.responseStatusCode();
  if (statusCode == 200) {

    String response = httpClient.responseBody();

    StaticJsonDocument<256> doc;
    DeserializationError error = deserializeJson(doc, response);

    if (error) {
      Serial.print("deserializeJson() failed: ");
      Serial.println(error.c_str());
      return;
    }

    int day = doc["day"];
    int month = doc["month"];
    int year = doc["year"];
    int hour = doc["hour"];
    int minutes = doc["minutes"];
    int seconds = doc["seconds"];
    int dayOfWeek = doc["dayOfWeek"];
    bool daylightSaving = doc["daylightSaving"];

    RTCTime initialTime(day, MonthFromInt(month), year, hour, minutes, seconds, DayOfWeekFromInt(dayOfWeek), SaveLightFromBool(daylightSaving));
    RTC.setTime(initialTime);

    Serial.println("Synced time:");
    // Serial.println(initialTime);
    RTCTime currenttime;
    RTC.getTime(currenttime);

    Serial.print("Current time: ");
    Serial.print(currenttime.getDayOfMonth());
    Serial.print("/");
    Serial.print(Month2int(currenttime.getMonth()));
    Serial.print("/");
    Serial.print(currenttime.getYear());
    Serial.print(" - ");
    Serial.print(currenttime.getHour());
    Serial.print(" : ");
    Serial.print(currenttime.getMinutes());
    Serial.print(" : ");
    Serial.println(currenttime.getSeconds());

  } else {
    Serial.println("Error, Statuscode:");
    Serial.println(statusCode);
  }

  httpClient.stop();
  httpClient.flush();
  delay(5000);
}

void GetTriggerInformation() {

  char getTriggerPath[] = "/Arduino/sensorTriggers";
  Serial.println("Fetchning trigger information");

  httpClient.beginRequest();
  httpClient.get(getTriggerPath);
  httpClient.sendHeader("ApiKey", SECRET_API_KEY);
  httpClient.endRequest();

  int statusCode = httpClient.responseStatusCode();
  if (statusCode == 200) {

    String response = httpClient.responseBody();

    StaticJsonDocument<384> getDoc;
    DeserializationError error = deserializeJson(getDoc, response);

    if (error) {
      Serial.print("deserializeJson() failed: ");
      Serial.println(error.c_str());
      return;
    }

    lightOnHour = getDoc["lightOnHour"];        // 8
    lightOnMinute = getDoc["lightOnMinute"];    // 0
    lightOffHour = getDoc["lightOffHour"];      // 18
    lightOffMinute = getDoc["lightOffMinute"];  // 0
    minTemp = getDoc["minTemp"];                // 10.15
    maxTemp = getDoc["maxTemp"];                // 16.22
    useSunLight = getDoc["useSunLight"];        // true
    sunUpHour = getDoc["sunUpHour"];            // 9
    sunUpMinute = getDoc["sunUpMinute"];        // 0
    sunDownHour = getDoc["sunDownHour"];        // 16
    sunDownMinute = getDoc["sunDownMinute"];    // 0

    Serial.println("Sync successful");

  } else {
    Serial.println("Error, Statuscode:");
    Serial.println(statusCode);
  }

  httpClient.stop();
  httpClient.flush();
  delay(5000);
}
#pragma endregion

#pragma region POST

void PostSensorValues(bool startup) {
  RTCTime currentTime;
  RTC.getTime(currentTime);

  int currentHour = currentTime.getHour();
  int currentMinute = currentTime.getMinutes();
  int intCurrentTime = currentHour * 100 + currentMinute;

  if ((currentMinute == 0 || currentMinute == 30) && intCurrentTime != lastSentSensorValues) {

    tempSensorIn.requestTemperatures();
    tempIndoorCelsius = tempSensorIn.getTempCByIndex(0);
    tempSensorOut.requestTemperatures();
    tempOutdoorCelsius = tempSensorOut.getTempCByIndex(0);

    signed char inWidth = 5;
    signed char outWidth = 5;
    signed char hourWidth = 1;
    signed char minuteWidth = 1;

    //set amount of characters based on the value
    if (tempIndoorCelsius < 0 || tempIndoorCelsius > 100) {
      inWidth = 6;
    }
    if (tempOutdoorCelsius < 0 || tempOutdoorCelsius > 100) {
      outWidth = 6;
    }
    if (currentHour > 9) {
      hourWidth = 2;
    }
    if (currentMinute > 9) {
      minuteWidth = 2;
    }

    char indoorTemp[10];
    char outdoorTemp[10];
    char hour[10];
    char minute[10];

    dtostrf(tempIndoorCelsius, inWidth, 2, indoorTemp);
    dtostrf(tempOutdoorCelsius, outWidth, 2, outdoorTemp);
    dtostrf(currentHour, hourWidth, 0, hour);
    dtostrf(currentMinute, minuteWidth, 0, minute);

    //create URL path
    char postSensorValuesPath[200];  // "/Arduino/sensorValues?innerTemp=" + indoorTemp + "&outerTemp=" + outdoorTemp + "&hour=18&minute=10";
    strcpy(postSensorValuesPath, "/Arduino/sensorValues?innerTemp=");
    strcat(postSensorValuesPath, indoorTemp);
    strcat(postSensorValuesPath, "&outerTemp=");
    strcat(postSensorValuesPath, outdoorTemp);
    strcat(postSensorValuesPath, "&hour=");
    strcat(postSensorValuesPath, hour);
    strcat(postSensorValuesPath, "&minute=");
    strcat(postSensorValuesPath, minute);

    Serial.println("Posting sensor values");
    Serial.println(postSensorValuesPath);

    httpClient.beginRequest();
    httpClient.post(postSensorValuesPath);
    httpClient.sendHeader("Content-Type", "application/json");
    httpClient.sendHeader("ApiKey", SECRET_API_KEY);
    httpClient.endRequest();

    int statusCode = httpClient.responseStatusCode();
    String response = httpClient.responseBody();
    if (statusCode == 200) {
      Serial.println("Sucessfully sent Sensor Values");
      lastSentSensorValues = intCurrentTime;
    } else {
      Serial.println("Error, Statuscore:");
      Serial.println(statusCode);
    }

    httpClient.stop();
    httpClient.flush();
    delay(5000);
  }
}

void PostLightSwitchChange(bool lightOn) {


  RTCTime currentTime;
  RTC.getTime(currentTime);
  int currentHour = currentTime.getHour();
  int currentMinute = currentTime.getMinutes();

  //set char count based on value
  signed char hourWidth = 1;
  signed char minuteWidth = 1;
  if (currentHour > 9) {
    hourWidth = 2;
  }
  if (currentMinute > 9) {
    minuteWidth = 2;
  }

  char hour[10];
  char minute[10];
  dtostrf(currentHour, hourWidth, 0, hour);
  dtostrf(currentMinute, minuteWidth, 0, minute);

  //convert bool to char, to send with request
  char* on = "false";
  if (lightOn) {
    on = "true";
  }

  //create URL path
  char postLightChangePath[200];
  strcpy(postLightChangePath, "/Arduino/light?lightOn=");
  strcat(postLightChangePath, on);
  strcat(postLightChangePath, "&hour=");
  strcat(postLightChangePath, hour);
  strcat(postLightChangePath, "&minute=");
  strcat(postLightChangePath, minute);

  Serial.println("Posting lightswitch change");
  Serial.println(postLightChangePath);

  httpClient.beginRequest();
  httpClient.post(postLightChangePath);
  httpClient.sendHeader("Content-Type", "application/json");
  httpClient.sendHeader("ApiKey", SECRET_API_KEY);
  httpClient.endRequest();

  int statusCode = httpClient.responseStatusCode();
  String response = httpClient.responseBody();
  if (statusCode == 200) {
    Serial.println("Sucessfully sent lightswitch change");
    lampOn = lightOn;
  } else {
    Serial.println("Error, Statuscore:");
    Serial.println(statusCode);
  }

  httpClient.stop();
  httpClient.flush();
  delay(5000);
}

void PostHeatSwitchChange(bool heaterOn) {

  RTCTime currentTime;
  RTC.getTime(currentTime);
  int currentHour = currentTime.getHour();
  int currentMinute = currentTime.getMinutes();

  //get indoor temp
  tempSensorIn.requestTemperatures();
  tempIndoorCelsius = tempSensorIn.getTempCByIndex(0);

  //set char count based on value
  signed char hourWidth = 1;
  signed char minuteWidth = 1;
  signed char inWidth = 5;
  if (currentHour > 9) {
    hourWidth = 2;
  }
  if (currentMinute > 9) {
    minuteWidth = 2;
  }
  if (tempIndoorCelsius < 0 || tempIndoorCelsius > 100) {
    inWidth = 6;
  }

  char hour[10];
  char minute[10];
  char indoorTemp[10];
  dtostrf(currentHour, hourWidth, 0, hour);
  dtostrf(currentMinute, minuteWidth, 0, minute);
  dtostrf(tempIndoorCelsius, inWidth, 2, indoorTemp);

  //convert bool to char, to send with request
  char* on = "false";
  if (heaterOn) {
    on = "true";
  }

  //create URL path
  char postHeatChangePath[200];
  strcpy(postHeatChangePath, "/Arduino/heat?heaterOn=");
  strcat(postHeatChangePath, on);
  strcat(postHeatChangePath, "&innerTemp=");
  strcat(postHeatChangePath, indoorTemp);
  strcat(postHeatChangePath, "&hour=");
  strcat(postHeatChangePath, hour);
  strcat(postHeatChangePath, "&minute=");
  strcat(postHeatChangePath, minute);

  Serial.println("Posting heatswitch change");
  Serial.println(postHeatChangePath);

  httpClient.beginRequest();
  httpClient.post(postHeatChangePath);
  httpClient.sendHeader("Content-Type", "application/json");
  httpClient.sendHeader("ApiKey", SECRET_API_KEY);
  httpClient.endRequest();


  int statusCode = httpClient.responseStatusCode();
  String response = httpClient.responseBody();
  if (statusCode == 200) {
    Serial.println("Sucessfully sent heatswitch change");
    heatOn = heaterOn;
  } else {
    Serial.println("Error, Statuscore:");
    Serial.println(statusCode);
  }
  httpClient.stop();
  httpClient.flush();
  delay(5000);
}

#pragma endregion

#pragma region Mappers

Month MonthFromInt(int month) {
  switch (month) {
    case 1:
      return Month::JANUARY;
    case 2:
      return Month::FEBRUARY;
    case 3:
      return Month::MARCH;
    case 4:
      return Month::APRIL;
    case 5:
      return Month::MAY;
    case 6:
      return Month::JUNE;
    case 7:
      return Month::JULY;
    case 8:
      return Month::AUGUST;
    case 9:
      return Month::SEPTEMBER;
    case 10:
      return Month::OCTOBER;
    case 11:
      return Month::NOVEMBER;
    case 12:
      return Month::DECEMBER;
  }
}

DayOfWeek DayOfWeekFromInt(int dayOfWeek) {
  switch (dayOfWeek) {
    case 0:
      return DayOfWeek::SUNDAY;
    case 1:
      return DayOfWeek::MONDAY;
    case 2:
      return DayOfWeek::TUESDAY;
    case 3:
      return DayOfWeek::WEDNESDAY;
    case 4:
      return DayOfWeek::THURSDAY;
    case 5:
      return DayOfWeek::FRIDAY;
    case 6:
      return DayOfWeek::SATURDAY;
  }
}

SaveLight SaveLightFromBool(bool saveLight) {
  if (saveLight) {
    return SaveLight::SAVING_TIME_ACTIVE;
  }
  return SaveLight::SAVING_TIME_INACTIVE;
}

#pragma endregion