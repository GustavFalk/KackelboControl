namespace KackelboControl_App.Models;

public class SensorTriggers
{
    public TimeOnly LightOnTime { get; set; }
    public TimeOnly LightOffTime { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
    public TimeOnly SunUpTime { get; set; }
    public TimeOnly SunDownTime { get; set; }
    public DateTime? ArduinoLastSync { get; set; }
}
