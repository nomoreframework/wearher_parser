using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace WeatherWebAppl.Models
{
    public class WeatherItem
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string Day { get; set; }
        public string Temperature { get; set; }
        public string WindSpeed { get; set; }
        public string Precipitation { get; set; }
        public string State { get; set; }
    }
}
