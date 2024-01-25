using KackelboControl_API.Models.Application;

namespace KackelboControl_API.Services;
public interface IApplicationService
{
    Task<SensorTriggersDto> GetSensorTriggers();
}

public class ApplicationService : IApplicationService
{
    private readonly IDateTimeProvider timeProvider;

    public ApplicationService(IDateTimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
    }

    public async Task<SensorTriggersDto> GetSensorTriggers()
    {
        await Task.Delay(1);
        return new SensorTriggersDto()
        {
            LightOnTime = TimeOnly.FromDateTime(timeProvider.SweTime()),
            LightOffTime = TimeOnly.FromDateTime(timeProvider.SweTime()),
            MinTemp = 18.55m,
            MaxTemp = 20.50m,
            UseSunLight = true,
            SunUpTime = TimeOnly.FromDateTime(timeProvider.SweTime()),
            SunDownTime = TimeOnly.FromDateTime(timeProvider.SweTime()),
        };
    }
}

