using KackelboControl_API.Database.Entities;

namespace KackelboControl_API.DTOs.Application;

public class SensorValueHistoryDto
{
    public SensorValueHistoryDto(List<TemperatureAndTimeDto> temperatureAndTimes, List<RelayDto> heatDtos, List<RelayDto> lightDtos)
    {
        TemparatureAndTime = temperatureAndTimes;
        HeatChange = heatDtos;
        LightChange = lightDtos;
    }

    public List<TemperatureAndTimeDto>? TemparatureAndTime { get; set; }
    public List<RelayDto> HeatChange { get; set; }
    public List<RelayDto> LightChange { get; set; }
}

public class TemperatureAndTimeDto
{
    public TemperatureAndTimeDto()
    {

    }
    public TemperatureAndTimeDto(SensorValue sensorValue)
    {
        LogTime = sensorValue.Created;
        IndoorTemperature = sensorValue.TempIndoors;
        OutdoorTemperature = sensorValue.TempOutdoor;
    }

    public DateTime LogTime { get; set; }
    public decimal IndoorTemperature { get; set; }
    public decimal OutdoorTemperature { get; set; }
}

public class RelayDto
{
    public RelayDto()
    {

    }

    public RelayDto(RelayChange relayChange)
    {
        LogTime = relayChange.Created;
        RelayOn = relayChange.RelayOn;

        if (relayChange.Relay == Relay.Heater)
        {
            IndoorTemperature = relayChange.TempIndoors;
        }
    }

    public DateTime LogTime { get; set; }

    public bool RelayOn { get; set; }

    public decimal? IndoorTemperature { get; set; }

}

