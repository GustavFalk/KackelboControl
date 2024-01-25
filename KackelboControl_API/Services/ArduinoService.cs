using KackelboControl_API.Models.Arduino;
using System;

namespace KackelboControl_API.Services
{
    public interface IArduinoService
    {
        Task<ArduinoSensorTriggersDto> GetArduinoSensorTriggers();
        Task PostArduinoHeatOn(bool heatOn, int hour, int minutes);
        Task PostArduinoLightOn(bool lightOn, int hour, int minutes);
        Task PostArduinoSensorValues(string innerTemp, string outerTemp, int hour, int minutes);
        TimeSyncDto SyncTime();
    }

    public class ArduinoService : IArduinoService
    {
        private readonly IDateTimeProvider timeProvider;

        public ArduinoService(IDateTimeProvider timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        public async Task<ArduinoSensorTriggersDto> GetArduinoSensorTriggers()
        {
            await Task.Delay(1);
            return new ArduinoSensorTriggersDto()
            {
                LightOnHour = 21,
                LightOnMinute = 00,
                LightOffHour = 21,
                LightOffMinute = 15,
                MinTemp = 18.15m,
                MaxTemp = 25.22m,
                UseSunLight = true,
                SunUpHour = 21,
                SunUpMinute = 05,
                SunDownHour = 21,
                SunDownMinute = 10
            };
        }

        public TimeSyncDto SyncTime()
        {
            var now = timeProvider.SweTime();
            var daylightSaving = timeProvider.IsDaylightSavingSwe();
            return new TimeSyncDto(now, daylightSaving);
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
