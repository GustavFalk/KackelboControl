using KackelboControl_API.Database.Entities;

namespace KackelboControl_API.Models.Application;

public class SensorTriggersDto
{

    public SensorTriggersDto()
    {

    }

    public SensorTriggersDto(SensorTriggers sensorTriggers, Sunlight sunHours)
    {
        LightOnTime = sensorTriggers.LightOnTime;
        LightOffTime = sensorTriggers.LightOffTime;
        MinTemp = sensorTriggers.MinTemp;
        MaxTemp = sensorTriggers.MaxTemp;
        UseSunLight = sensorTriggers.UseSunLight;
        SunUpTime = sunHours.Sunrise;
        SunDownTime = sunHours.Sunset;        
    }

    public TimeOnly LightOnTime { get; set; }
    public TimeOnly LightOffTime { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
    public TimeOnly SunUpTime { get; set; }
    public TimeOnly SunDownTime { get; set; }

}
