using KackelboControl_API.Database;
using KackelboControl_API.Database.Entities;
using KackelboControl_API.DTOs.Application;
using KackelboControl_API.Models.Application;
using Microsoft.EntityFrameworkCore;

namespace KackelboControl_API.Services;
public interface IApplicationService
{
    Task<SensorTriggersDto> GetSensorTriggers();
    Task<SensorValueHistoryDto> GetSensorValuesHistory(DateTime forDate);
    Task PostSensorTriggers(UpdateSensorTriggersDto updateSensorTriggers);
}

public class ApplicationService : IApplicationService
{
    private readonly IDateTimeProvider timeProvider;
    private readonly KackelboControlDbContext dbContext;

    public ApplicationService(IDateTimeProvider timeProvider, KackelboControlDbContext dbContext)
    {
        this.timeProvider = timeProvider;
        this.dbContext = dbContext;
    }

    public async Task<SensorTriggersDto> GetSensorTriggers()
    {
        var sensorTriggers = await dbContext.SensorTriggers
              .OrderByDescending(x => x.Created)
              .FirstOrDefaultAsync();

        var sunHours = await dbContext.SunRiseSunSet
            .Where(x => x.Date == DateOnly.FromDateTime(timeProvider.SweTime()))
            .FirstOrDefaultAsync();

        if (sensorTriggers != null && sunHours != null)
        {
            return new SensorTriggersDto(sensorTriggers, sunHours);
        }

        return new SensorTriggersDto();
    }

    public async Task<SensorValueHistoryDto> GetSensorValuesHistory(DateTime forDate)
    {
        var historyPosts = await dbContext.SensorValueLog
            .Where(x => x.Created.Date == forDate.Date)
            .Select(x => new TemperatureAndTimeDto(x))
            .ToListAsync();

        var relayChanges = await dbContext.RelayChangeLog
            .Where(x => x.Created.Date == forDate.Date)
            .ToListAsync();

        var lightChanges = relayChanges
            .Where(x => x.Relay == Relay.Lamp)
            .Select(x => new RelayDto(x))
            .ToList();

        var heatChanges = relayChanges
           .Where(x => x.Relay == Relay.Heater)
           .Select(x => new RelayDto(x))
           .ToList();

        return new SensorValueHistoryDto(historyPosts, heatChanges, lightChanges);
    }

    public async Task PostSensorTriggers(UpdateSensorTriggersDto updateSensorTriggers)
    {
        var updatedSensorTriggers = new SensorTriggers(updateSensorTriggers);

        dbContext.Add(updatedSensorTriggers);
        await dbContext.SaveChangesAsync();
    }
}

