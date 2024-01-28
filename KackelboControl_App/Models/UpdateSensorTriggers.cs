namespace KackelboControl_App.Models;

public class UpdateSensorTriggers
{
    public UpdateSensorTriggers()
    {

    }
    public UpdateSensorTriggers(SensorTriggers sensorTriggers)
    {
        LightOnTime = sensorTriggers.LightOnTime;
        LightOffTime = sensorTriggers.LightOffTime;
        MinTemp = sensorTriggers.MinTemp;
        MaxTemp = sensorTriggers.MaxTemp;
        UseSunLight = sensorTriggers.UseSunLight;
    }
    public TimeOnly LightOnTime { get; set; }
    public TimeOnly LightOffTime { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public bool UseSunLight { get; set; }
}
