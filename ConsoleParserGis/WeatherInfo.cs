using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ConsoleParserGis
{
    public class WeatherInfo
    {
        public string CityName { get; }
        public string ResourceLink { get;}
        public List<WeatherItem> weatherItems { get; }
        JsonSerializerOptions op;

        internal WeatherInfo(string city_name, string resourceLink, List<WeatherItem> wetheritems)
        {
            CityName = city_name;
            ResourceLink = resourceLink;
            weatherItems = new List<WeatherItem>();
            weatherItems.AddRange(wetheritems);
            op = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin,
                                                     UnicodeRanges.Cyrillic,
                                                     UnicodeRanges.Specials,
                                                     UnicodeRanges.NumberForms),
                WriteIndented = true
            };

        }

        internal string GetWeatherAsJson(WeatherInfo info, JsonSerializerOptions options = null)
        {
            if (options == null) options = op;
            string weather_json = JsonSerializer.Serialize<WeatherInfo>(info, options);
            return weather_json;
        }

    }
}
