using KackelboControl_API.Authentication;
using KackelboControl_API.Models.Arduino;
using KackelboControl_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace KackelboControl_API.Controllers;

[ApiController]
[Route("[controller]")]
[ApiKeyAuth]
public class ArduinoController : ControllerBase
{
    private readonly IArduinoService arduinoService;
    private readonly ILogger logger;

    public ArduinoController(IArduinoService arduinoService, ILogger<ArduinoController> logger)
    {
        this.arduinoService = arduinoService;
        this.logger = logger;
    }

    [HttpGet("sensorTriggers")]
    public async Task<ArduinoSensorTriggersDto> GetSensorTriggers()
    {
        try
        {
            var triggers = await arduinoService.GetArduinoSensorTriggers();
            return triggers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("timeSync")]
    public TimeSyncDto TimeSync()
    {
        var time = arduinoService.SyncTime();
        return time;
    }

    [HttpPost("sensorValues")]
    public async Task<IActionResult> PostSensorValues(string innerTemp, string outerTemp, int hour, int minute)
    {
        try
        {
            await arduinoService.PostArduinoSensorValues(innerTemp, outerTemp, hour, minute);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("light")]
    public async Task<IActionResult> PostLightOn(bool lightOn, int hour, int minute)
    {
        try
        {

            await arduinoService.PostArduinoLightOn(lightOn, hour, minute);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("heat")]
    public async Task<IActionResult> PostHeatOn(bool heatOn, string innerTemp, int hour, int minute)
    {
        try
        {
            await arduinoService.PostArduinoHeatOn(heatOn, innerTemp, hour, minute);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }
}
