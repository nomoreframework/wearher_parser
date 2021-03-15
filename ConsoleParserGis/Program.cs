using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ConsoleParserGis
{
    public class MongoItem
    {
        public ObjectId Id { get; set; }
        public List<WeatherInfo> weatherInfos { get; set; }
    }
    public class Program
    {
        const string URL = "https://www.gismeteo.ru";
        const string suffix = "10-days";
        static List<WeatherInfo> weathers;
        const string connectionString = "mongodb://localhost";
        static MongoItem item;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Parser was started at: ");
                PrintBl(DateTime.Now.ToShortTimeString().ToString());
                try
                {
                    var task = Task.Factory.StartNew(() => get_data_parallel(), TaskCreationOptions.LongRunning);
                    Console.WriteLine("Working...");
                    Task.WaitAll(task);
                    foreach (var el in weathers) el.DateOfLastUpdate = DateTime.Now.ToLocalTime().ToString();
                    item = new MongoItem();
                    item.weatherInfos = weathers;
                }
                catch (AggregateException ex)
                {
                    PrintErr(ex.StackTrace);
                    continue;
                }
                catch (Exception ex)
                {           
                    PrintErr(ex.StackTrace);
                    continue;
                }
                Print("Parser completed successfully! ");
                PrintBl(DateTime.Now.ToShortTimeString().ToString());
                Console.WriteLine("Startt saving into the database: ");
                PrintBl(DateTime.Now.ToShortTimeString().ToString());
                try 
                {
                    SaveWeatherDoc();
                }
                 catch(Exception ex)
                {
                    PrintErr(ex.Message + DateTime.Now.ToShortTimeString().ToString());
                    continue;
                }
             
                    Print("Added into database was completed successfully! ");

                Console.WriteLine("sleep state..." + "\n");
                PrintBl("For exit - pres 'Ctrl + C'");
                Thread.Sleep(new Random().Next(300 * 1000, 600 * 1000));
            }

        }

        static void get_data_synch()
        {
            weathers = new Worker(URL, suffix).ParseSynchronus().Result;
        }
        static void get_data_parallel()
        {
            weathers = new Worker(URL, suffix).ParseParallel().Result;
        }
        private static void SaveWeatherDoc()
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<MongoItem>("weatherscoll");

            collection.InsertOne(item);
        }
        internal static void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        internal static void PrintErr(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        internal static void PrintBl(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(message + "\n");
            Console.ResetColor();
        }
    }
}
