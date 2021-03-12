using MongoDB.Bson;
using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace WeatherWebAppl.Models
{
    public class WeatherInfo
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string CityName { get; set; }
        public string ResourceLink { get; set; }
        public List<WeatherItem> weatherItems { get; set; }
        public string DateOfLastUpdate { get; set; }

        internal WeatherInfo(string city_name, string resourceLink, List<WeatherItem> wetheritems)
        {
            CityName = city_name;
            ResourceLink = resourceLink;
            weatherItems = new List<WeatherItem>();
            weatherItems.AddRange(wetheritems);
        }
    }
}

