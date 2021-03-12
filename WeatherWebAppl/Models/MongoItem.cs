using MongoDB.Bson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WeatherWebAppl.Models
{
    public class MongoItem
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public List<WeatherInfo> weatherInfos { get; set; }
    }

}
