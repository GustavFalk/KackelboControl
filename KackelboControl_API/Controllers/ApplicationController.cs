using KackelboControl_API.Authentication;
using KackelboControl_API.DTOs.Application;
using KackelboControl_API.Models.Application;
using KackelboControl_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace KackelboControl_API.Controllers;


[ApiController]
[Route("[controller]")]
[ApiKeyAuth]
public class ApplicationController : ControllerBase
{
    private readonly ILogger<ApplicationController> logger;
    private readonly IDateTimeProvider timeProvider;
    private readonly IApplicationService applicationService;

    public ApplicationController(ILogger<ApplicationController> logger, IDateTimeProvider timeProvider, IApplicationService applicationService)
    {
        this.logger = logger;
        this.timeProvider = timeProvider;
        this.applicationService = applicationService;
    }


    [HttpGet("sensorTriggers")]
    public async Task<SensorTriggersDto> GetSensorTriggers()
    {
        try
        {
            var triggers = await applicationService.GetSensorTriggers();
            return triggers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("latestSensorValues")]
    public async Task<SensorValueHistoryDto> GetSensorValueHistory()
    {
        try
        {
            var history = await applicationService.GetSensorValuesHistory();
            return history;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("sensorValueHistory")]
    public async Task<SensorValueHistoryDto> GetSensorValueHistory(DateTime forDate)
    {
        try
        {
            var history = await applicationService.GetSensorValuesHistory(forDate);
            return history;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("eggCount")]
    public async Task<int> EggCount(DateOnly forDate)
    {
        try
        {
            var eggCount = await applicationService.GetEggCount(forDate);
            return eggCount;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("eggCountLog")]
    public async Task<List<EggCountDto>> GetEggCountLog()
    {
        try
        {
            var eggCountLog = await applicationService.GetEggCountLog();
            return eggCountLog;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpPost("eggCount")]
    public async Task<IActionResult> EggCount(EggCountDto eggCount)
    {
        try
        {            
            await applicationService.PostEggCount(eggCount);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("sensorTriggers")]
    public async Task<SensorTriggersDto> PostSensorTrigger(UpdateSensorTriggersDto updateSensorTriggers)
    {
        try
        {
            var updatedTriggers = await applicationService.PostSensorTriggers(updateSensorTriggers);
            return updatedTriggers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

}