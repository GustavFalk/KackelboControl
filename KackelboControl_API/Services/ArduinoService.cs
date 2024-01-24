using KackelboControl_API.Models.Arduino;

namespace KackelboControl_API.Services
{
    public interface IArduinoService
    {
        Task<SensorTriggers> GetArduinoSensorTriggers();
        Task PostArduinoHeatOn(bool heatOn, int hour, int minutes);
        Task PostArduinoLightOn(bool lightOn, int hour, int minutes);
        Task PostArduinoSensorValues(string innerTemp, string outerTemp, int hour, int minutes);
    }

    public class ArduinoService : IArduinoService
    {
        public ArduinoService()
        {
            
        }

        public async Task<SensorTriggers> GetArduinoSensorTriggers()
        {
            await Task.Delay(1);
            return new SensorTriggers()
            {
                LightOnHour = 21,
                LightOnMinute = 00,
                LightOffHour = 21,
                LightOffMinute =  15,
                MinTemp =  18.15m,
                MaxTemp = 25.22m,
                UseSunLight = true,
                SunUpHour = 21,
                SunUpMinute = 05,
                SunDownHour = 21,
                SunDownMinute = 10
            };
        }

        public async Task PostArduinoSensorValues(string innerTemp, string outerTemp, int hour, int minute)
        {
            TimeOnly arduinoTime = new TimeOnly(hour, minute);
            await Task.Delay(1);
        }


        public async Task PostArduinoLightOn(bool lightOn, int hour, int minute)
        {
            TimeOnly arduinoTime = new TimeOnly(hour, minute);
            await Task.Delay(1);
        }

        public async Task PostArduinoHeatOn(bool heatOn, int hour, int minute)
        {
            TimeOnly arduinoTime = new TimeOnly(hour, minute);
            await Task.Delay(1);
        }
    } 


}
