using KackelboControl_API.Database.Entities;
using Newtonsoft.Json;
using System.Globalization;

namespace KackelboControl_API.Helpers;

public static class LocalJsonReader
{

    public static List<Sunlight> GetSunlightFromJson()
    {
        List<Sunlight> sunlightList = new List<Sunlight>();

        string jsonSun = File.ReadAllText("./Helpers/sunrisesunset.json");
        List<Suntime> parsedJson = JsonConvert.DeserializeObject<List<Suntime>>(jsonSun);

        int id = 1;
        foreach (var suntime in parsedJson)
        {

            string rise = DateTime.Parse(suntime.sunrise).ToString("HH:mm");
            string set = DateTime.Parse(suntime.sunset).ToString("HH:mm");
            sunlightList.Add(new Sunlight()
            {
                Id = id,
                Date = DateOnly.Parse(suntime.date),
                Sunrise = TimeOnly.ParseExact(rise, "HH:mm", CultureInfo.InvariantCulture),
                Sunset = TimeOnly.ParseExact(set, "HH:mm", CultureInfo.InvariantCulture)
            });

            id++;
        }

        return sunlightList;
    }


    private class Suntime
    {
        public string date { get; set; }

        public string sunrise { get; set; }

        public string sunset { get; set; }

    }
}
