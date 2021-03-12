using MongoDB.Bson;
using System.Collections.Generic;
using System;


namespace ConsoleParserGis
{
    public class WeatherInfo
    {
        public ObjectId Id { get; set; }
        public string CityName { get; set; }
        public string ResourceLink { get; set; }
        public List<WeatherItem> weatherItems { get; set; }
        public string DateOfLastUpdate { get; set; }
      //  JsonSerializerOptions op;

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
