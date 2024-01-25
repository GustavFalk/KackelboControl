namespace KackelboControl_API.DTOs.Application;

public class UpdateSensorTriggersDto
{
    public TimeOnly LightOnTime { get; set; }
    public TimeOnly LightOffTime { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
}
