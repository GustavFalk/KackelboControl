﻿using KackelboControl_API.Database;
using KackelboControl_API.Database.Entities;
using KackelboControl_API.DTOs.Application;
using KackelboControl_API.Models.Application;
using Microsoft.EntityFrameworkCore;

namespace KackelboControl_API.Services;
public interface IApplicationService
{
    Task<int> GetEggCount(DateOnly forDate);
    Task<List<EggCountDto>> GetEggCountLog();
    Task<SensorTriggersDto> GetSensorTriggers();
    Task<SensorValueHistoryDto> GetSensorValuesHistory();
    Task<SensorValueHistoryDto> GetSensorValuesHistory(DateTime forDate);
    Task PostEggCount(EggCountDto eggCountDto);
    Task<SensorTriggersDto> PostSensorTriggers(UpdateSensorTriggersDto updateSensorTriggers);
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

    public async Task<SensorValueHistoryDto> GetSensorValuesHistory()
    {
        var historyPosts = await dbContext.SensorValueLog
            .Where(x => x.Created.Date > timeProvider.SweTime().AddDays(-1))
            .Select(x => new TemperatureAndTimeDto(x))
            .ToListAsync();

        var relayChanges = await dbContext.RelayChangeLog
            .Where(x => x.Created.Date > timeProvider.SweTime().AddDays(-1))
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

    public async Task<int> GetEggCount(DateOnly forDate)
    {
        var eggLogPost = await dbContext.EggCountLog
            .Where(x => x.CountDate == forDate)
            .FirstOrDefaultAsync();

        if (eggLogPost == null)
        {
            return 0;
        }
        return eggLogPost.Count;

    }


    public async Task<List<EggCountDto>> GetEggCountLog()
    {
        List<EggCountDto> eggsFromOneWeek = new List<EggCountDto>();

        var today = DateOnly.FromDateTime(timeProvider.SweTime());
        var oneWeekBack = today.AddDays(-6);
        

        eggsFromOneWeek = await dbContext.EggCountLog
             .Where(x => x.CountDate > oneWeekBack)
             .Select(x => new EggCountDto(x))
             .ToListAsync();

        List<EggCountDto> eggLog = new();

        DateOnly currentDay = oneWeekBack;
        for (var day = 0; currentDay < today; day++)
        {
            
            currentDay = oneWeekBack.AddDays(day);
         
            EggCountDto logPost = new()
            {
                CountDate = currentDay,
                Count = 0
            };

            if(eggsFromOneWeek.Where(x=>x.CountDate== currentDay).Any())
            {
                logPost.Count = eggsFromOneWeek.Where(x => x.CountDate == currentDay).OrderByDescending(x=>x.CountDate).FirstOrDefault().Count;
            }
            
            eggLog.Add(logPost);
        }

        return eggLog;
    }

    public async Task PostEggCount(EggCountDto eggCountDto)
    {

        var logFromSameDay = await dbContext.EggCountLog
             .Where(x => x.CountDate == eggCountDto.CountDate)
             .FirstOrDefaultAsync();

        if (logFromSameDay == null)
        {
            dbContext.Add(new EggCount(eggCountDto));
        }
        else
        {
            logFromSameDay.Count = eggCountDto.Count;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<SensorTriggersDto> PostSensorTriggers(UpdateSensorTriggersDto updateSensorTriggers)
    {
        var updatedSensorTriggers = new SensorTriggers(updateSensorTriggers, timeProvider.SweTime());
       
        dbContext.Add(updatedSensorTriggers);
        await dbContext.SaveChangesAsync();

        return await GetSensorTriggers();
    }
}

