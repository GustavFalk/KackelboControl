using KackelboControl_API.Authentication;
using KackelboControl_API.Helpers;
using KackelboControl_API.Models.Application;
using KackelboControl_API.Models.Arduino;
using KackelboControl_API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

}