using MongoDB.Bson;

namespace ConsoleParserGis
{
    public class WeatherItem
    {
        public ObjectId Id { get; set; }
        public string Day { get; set; }
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
        public string WindSpeed { get; set; }
        public string Precipitation { get; set; }
        public string State { get; set; }
    }
}
