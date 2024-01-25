using KackelboControl_API.Database;
using KackelboControl_API.Models.Arduino;
using Microsoft.EntityFrameworkCore;

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
        private readonly KackelboControlDbContext dbContext;

        public ArduinoService(IDateTimeProvider timeProvider, KackelboControlDbContext dbContext)
        {
            this.timeProvider = timeProvider;
            this.dbContext = dbContext;
        }

        public async Task<ArduinoSensorTriggersDto> GetArduinoSensorTriggers()
        {

            var sunHours = await dbContext.SunRiseSunSet
                .Where(x => x.Date == DateOnly.FromDateTime(timeProvider.SweTime()))
                .FirstOrDefaultAsync();

            return new ArduinoSensorTriggersDto()
            {
                LightOnHour = 21,
                LightOnMinute = 00,
                LightOffHour = 21,
                LightOffMinute = 15,
                MinTemp = 18.15m,
                MaxTemp = 25.22m,
                UseSunLight = true,
                SunUpHour = sunHours.Sunrise.Hour,
                SunUpMinute = sunHours.Sunrise.Minute,
                SunDownHour = sunHours.Sunset.Hour,
                SunDownMinute = sunHours.Sunset.Minute
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
