using KackelboControl_API.Models;

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
                LightOn = new TimeOnly(08, 00),
                LightOff = new TimeOnly(18, 00),
                MinTemp = 10,
                MaxTemp = 16,
                UseSunLight = true,
                SunUp = new TimeOnly(09, 00),
                SunDown = new TimeOnly(16, 00)
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
