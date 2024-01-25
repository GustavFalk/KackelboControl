using KackelboControl_API.Services;
using System.Globalization;

namespace KackelboControl_API.Models.Arduino;

public class TimeSyncDto
{   
    public TimeSyncDto(DateTime now, bool daylightSaving)
    {     
        Day = now.Date.Day;
        Month = now.Date.Month;
        Year = now.Date.Year;
        Hour = now.Hour;
        Minutes = now.Minute;
        Seconds = now.Second;
        DayOfWeek = (int)now.Date.DayOfWeek;
        DaylightSaving = daylightSaving;
    }

    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int Hour { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public int DayOfWeek { get; set; }
    public bool DaylightSaving { get; set; }


}
