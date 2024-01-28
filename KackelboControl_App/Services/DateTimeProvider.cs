namespace KackelboControl_App.Services;

public interface IDateTimeProvider
{
    bool IsDaylightSavingSwe();
    DateTime SweTime();
}

public class DateTimeProvider : IDateTimeProvider
{
    private static readonly TimeZoneInfo sweTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

    public DateTime SweTime() => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sweTimeZone);

    public bool IsDaylightSavingSwe() => sweTimeZone.IsDaylightSavingTime(SweTime());

}


