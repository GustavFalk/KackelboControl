using KackelboControl_API.Authentication;
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
    public WeatherForecast GetAPA(string farbror)
    {
        return new WeatherForecast()
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };

    }
}
