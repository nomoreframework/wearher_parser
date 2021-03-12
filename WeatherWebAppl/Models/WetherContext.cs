using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherWebAppl.Models;

namespace WeatherWebAppl.Models
{
    public class WetherContext
    {
        IGridFSBucket gridFS;   // файловое хранилище
        IMongoCollection<MongoItem> Weathers; // коллекция в базе данных
        public WetherContext()
        {
            // строка подключения
            string connectionString = "mongodb://localhost:27017/test";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
            // обращаемся к коллекции Products
            Weathers = database.GetCollection<MongoItem>("weatherscoll");
        }
        public async Task<List<WeatherInfo>> GetWeather()
        {
           List<MongoItem> weathers = await Weathers.Find(new BsonDocument()).ToListAsync();
            return weathers.Last().weatherInfos;
        }
    }
}
