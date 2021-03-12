using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleParserGis
{
    internal class Worker : IWorker
    {
        public Task[] ParserTasks;

        public int TasksArraySize { get; }
        public ParserSettings settings { get; }
        public Dictionary<string, string> refs_dictionary;
        List<WeatherInfo> weatherInfos;

        internal Worker(string URL, string suffix)
        {
            settings = new ParserSettings(URL, suffix, "noscript", "href", "data-name", "a");
            weatherInfos = new List<WeatherInfo>();
        }

        internal async Task<List<WeatherInfo>>  ParseParallel()
        {
            int index = 0;
            refs_dictionary = await settings.GetCitiesRefrs();
            ParserTasks = new Task[refs_dictionary.Count];
            foreach (var e in refs_dictionary)
            {
                HttpResponseMessage mes = await settings.client.GetAsync(e.Value);
                string result = await mes.Content.ReadAsStringAsync();
                Task t = new Task(() => get_data(e.Key, e.Value, settings, result, mes));
                ParserTasks[index] = t;
                index++;
            }
            foreach (var t in ParserTasks) t.Start();
            Task.WaitAll(ParserTasks);
            return weatherInfos;
        }
        internal async Task<List<WeatherInfo>> ParseSynchronus()
        {
            refs_dictionary = await settings.GetCitiesRefrs();
            foreach (var e in refs_dictionary)
            {
                HttpResponseMessage mes = await settings.client.GetAsync(e.Value);
                string result = await mes.Content.ReadAsStringAsync();
                get_data(e.Key, e.Value, settings, result, mes);
            }
            return weatherInfos;
        }
         void get_data(string key, string value, ParserSettings obj, string result, HttpResponseMessage mes)
        {
            var weather = new WeatherParser(result);
            var w = new WeatherInfo(key, value, weather.GetWeathers());
            weatherInfos.Add(w);
        }
    }
}
