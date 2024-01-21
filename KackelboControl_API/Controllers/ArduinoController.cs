using KackelboControl_API.Authentication;
using KackelboControl_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace KackelboControl_API.Controllers;

[ApiController]
[Route("[controller]")]
[ApiKeyAuth]
public class ArduinoController : ControllerBase
{
    private readonly ILogger<ArduinoController> _logger;

    public ArduinoController(ILogger<ArduinoController> logger)
    {
        _logger = logger;
    }

    [HttpGet("sensorTriggers")]
    public SensorTriggers GetSensorTriggers()
    {
        return new SensorTriggers()
        {
            LightOn = new TimeOnly(08, 00),
            LightOff = new TimeOnly(18, 00),
            MinTemp = 10,
            MaxTemp = 16,
            UseSunLight = true,
            SunUp = new TimeOnly(09, 00),
            SunDown = new TimeOnly(16, 00)
        };
    }

    [HttpPost("sensorValues")]
    public async Task<IActionResult> PostSensorValues(string innerTemp, string outerTemp, int hour, int minute)
    {
        TimeOnly arduinoTime = new TimeOnly(hour, minute);
        await Task.Delay(1);
        return StatusCode(200);
    }

    [HttpPost("light")]
    public async Task<IActionResult> PostLightOn(bool lightOn, int hour, int minute)
    {
        TimeOnly arduinoTime = new TimeOnly(hour, minute);
        await Task.Delay(1);
        return StatusCode(200);
    }

    [HttpPost("heat")]
    public async Task<IActionResult> PostHeatOn(bool heatOn, int hour, int minute)
    {
        TimeOnly arduinoTime = new TimeOnly(hour, minute);
        await Task.Delay(1);
        return StatusCode(200);
    }
}
