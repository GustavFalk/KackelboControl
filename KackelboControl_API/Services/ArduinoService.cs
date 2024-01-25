using KackelboControl_API.Database;
using KackelboControl_API.Database.Entities;
using KackelboControl_API.Models.Arduino;
using Microsoft.EntityFrameworkCore;

namespace KackelboControl_API.Services
{
    public interface IArduinoService
    {
        Task<ArduinoSensorTriggersDto> GetArduinoSensorTriggers();
        Task PostArduinoHeatOn(bool heatOn, string innerTemp, int hour, int minutes);
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

            var sensorTriggers = await dbContext.SensorTriggers
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            var sunHours = await dbContext.SunRiseSunSet
                .Where(x => x.Date == DateOnly.FromDateTime(timeProvider.SweTime()))
                .FirstOrDefaultAsync();

            if (sensorTriggers != null && sunHours != null)
            {
                sensorTriggers.LastSynced = timeProvider.SweTime();
                dbContext.Update(sensorTriggers);
                await dbContext.SaveChangesAsync();

                return new ArduinoSensorTriggersDto(sensorTriggers, sunHours);
            }

            return new ArduinoSensorTriggersDto();

        }

        public TimeSyncDto SyncTime()
        {
            var now = timeProvider.SweTime();
            var daylightSaving = timeProvider.IsDaylightSavingSwe();
            return new TimeSyncDto(now, daylightSaving);
        }

        public async Task PostArduinoSensorValues(string innerTemp, string outerTemp, int hour, int minute)
        {
            var sensorValues = new SensorValue()
            {
                TempIndoors = Convert.ToDecimal(innerTemp),
                TempOutdoor = Convert.ToDecimal(outerTemp),
                Created = timeProvider.SweTime(),
                ArduinoTime = new TimeOnly(hour, minute)
            };

            dbContext.Add(sensorValues);
            await dbContext.SaveChangesAsync();
        }


        public async Task PostArduinoLightOn(bool lightOn, int hour, int minute)
        {
            var lightOnChange = new RelayChange()
            {
                Created = timeProvider.SweTime(),
                ArduinoTime = new TimeOnly(hour, minute),
                Relay = Relay.Lamp,
                RelayOn = lightOn
            };

            dbContext.Add(lightOnChange);
            await dbContext.SaveChangesAsync();
        }

        public async Task PostArduinoHeatOn(bool heatOn, string innerTemp, int hour, int minute)
        {
            var heatOnChange = new RelayChange()
            {
                Created = timeProvider.SweTime(),
                ArduinoTime = new TimeOnly(hour, minute),
                Relay = Relay.Heater,
                RelayOn = heatOn,
                TempIndoors = Convert.ToDecimal(innerTemp)
            };

            dbContext.Add(heatOnChange);
            await dbContext.SaveChangesAsync();
        }
    }


}
