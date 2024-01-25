namespace KackelboControl_API.Models.Application;

public class SensorTriggersDto
{
    public TimeOnly LightOnTime { get; set; }
    public TimeOnly LightOffTime { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
    public TimeOnly SunUpTime { get; set; }
    public TimeOnly SunDownTime { get; set; }

}
