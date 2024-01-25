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

    [HttpPost("sensorTriggers")]
    public async Task<IActionResult> PostSensorTrigger(UpdateSensorTriggersDto updateSensorTriggers)
    {
        try
        {
            await applicationService.PostSensorTriggers(updateSensorTriggers);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

}