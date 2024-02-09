using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KackelboControl_App.Services;

public static class Helpers
{

    public static string DayToSwe(string day)
    {
        switch (day)
        {
            case "Monday":
                return "MÅN";
            case "Tuesday":
                return "TIS";
            case "Wednesday":
                return "ONS";
            case "Thursday":
                return "TOR";
            case "Friday":
                return "FRE";
            case "Saturday":
                return "LÖR";
            case "Sunday":
                return "SÖN";
            default:
                return "";
        }
    }
}
