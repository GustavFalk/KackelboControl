using KackelboControl_API.Database.Entities;

namespace KackelboControl_API.Models.Arduino;

public class ArduinoSensorTriggersDto
{

    public ArduinoSensorTriggersDto()
    {
        
    }
    public ArduinoSensorTriggersDto(SensorTriggers triggers, Sunlight sunHours)
    {
        LightOnHour = triggers.LightOnTime.Hour;
        LightOnMinute = triggers.LightOnTime.Minute;
        LightOffHour = triggers.LightOffTime.Hour;
        LightOffMinute = triggers.LightOffTime.Minute;
        MinTemp = triggers.MinTemp;
        MaxTemp = triggers.MaxTemp;
        UseSunLight = triggers.UseSunLight;
        SunUpHour = sunHours.Sunrise.Hour;
        SunUpMinute = sunHours.Sunrise.Minute;
        SunDownHour = sunHours.Sunset.Hour;
        SunDownMinute = sunHours.Sunset.Minute;

    }
    public int LightOnHour { get; set; }
    public int LightOnMinute { get; set; }
    public int LightOffHour { get; set; }
    public int LightOffMinute { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
    public int SunUpHour { get; set; }
    public int SunUpMinute { get; set; }
    public int SunDownHour { get; set; }
    public int SunDownMinute { get; set; }

}
