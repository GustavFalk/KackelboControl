namespace KackelboControl_API.Models;

public class SensorTriggers
{
    public TimeOnly LightOn { get; set; }
    public TimeOnly LightOff { get; set; }
    public long MinTemp { get; set; }
    public long MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
    public TimeOnly SunUp { get; set; }
    public TimeOnly SunDown { get; set; }

}
