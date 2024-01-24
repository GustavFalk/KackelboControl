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
    private readonly IArduinoService _arduinoService;
    private readonly ILogger _logger;
    private readonly IDateTimeProvider _timeProvider;
    public ArduinoController(IArduinoService arduinoService, ILogger<ArduinoController> logger, IDateTimeProvider timeProvider)
    {
        _arduinoService = arduinoService;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    [HttpGet("sensorTriggers")]
    public async Task<SensorTriggers> GetSensorTriggers()
    {
        try
        {
            var triggers = await _arduinoService.GetArduinoSensorTriggers();
            return triggers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("timeSync")]
    public TimeSync TimeSync()
    {
        var now = _timeProvider.SweTime();
        var daylightSaving = _timeProvider.IsDaylightSavingSwe();
        return new TimeSync(now,daylightSaving);
    }

    [HttpPost("sensorValues")]
    public async Task<IActionResult> PostSensorValues(string innerTemp, string outerTemp, int hour, int minute)
    {
        try
        {
            await _arduinoService.PostArduinoSensorValues(innerTemp, outerTemp, hour, minute);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }

    }

    [HttpPost("light")]
    public async Task<IActionResult> PostLightOn(bool lightOn, int hour, int minute)
    {
        try
        {

            await _arduinoService.PostArduinoLightOn(lightOn, hour, minute);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("heat")]
    public async Task<IActionResult> PostHeatOn(bool heatOn, string innerTemp, int hour, int minute)
    {
        try
        {
            await _arduinoService.PostArduinoHeatOn(heatOn, hour, minute);
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }
}
