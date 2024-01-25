namespace KackelboControl_API.Models.Arduino;

public class ArduinoSensorTriggersDto
{
    public int LightOnHour { get; set; }
    public int LightOnMinute { get; set; }
    public int LightOffHour { get; set; }
    public int LightOffMinute { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
    public int SunUpHour { get; set; }
    public int SunUpMinute { get; set; }
    public int SunDownHour { get; set; }
    public int SunDownMinute { get; set; }

}
