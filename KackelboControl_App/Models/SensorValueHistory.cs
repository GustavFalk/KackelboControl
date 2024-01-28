namespace KackelboControl_App.Models;

public class SensorValueHistory
{
    public List<TemperatureAndTime> TemparatureAndTime { get; set; } = new List<TemperatureAndTime>();
    public List<Relay> HeatChange { get; set; } = new List<Relay>();
    public List<Relay> LightChange { get; set; } = new List<Relay>();
}

public class TemperatureAndTime
{
    public DateTime LogTime { get; set; }
    public decimal IndoorTemperature { get; set; }
    public decimal OutdoorTemperature { get; set; }
}

public class Relay
{

    public DateTime LogTime { get; set; }

    public bool RelayOn { get; set; }

    public decimal? IndoorTemperature { get; set; }
}