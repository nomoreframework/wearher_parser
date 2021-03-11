using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleParserGis
{
    internal class Worker : IWorker
    {
        public Task[] ParserTasks;

        public int TasksArraySize { get; }

        internal void ParseParallel(Dictionary<string, string> dict, ParserSettings obj)
        {
            int index = 0;
            ParserTasks = new Task[dict.Count];
            foreach (var e in dict)
            {
                  Task t = new Task(() => get_data(e.Key, e.Value, obj));
                ParserTasks[index] = t;
                index++;
            }
            foreach (var t in ParserTasks) t.Start();
            Task.WaitAll(ParserTasks);
        }
        internal void ParseSynchronus(Dictionary<string, string> dict, ParserSettings obj)
        {
            foreach (var e in dict)
            {
                get_data(e.Key, e.Value, obj);
            }
        }
        async void get_data(string key, string value, ParserSettings obj)
        {
            HttpResponseMessage mes = await obj.client.GetAsync(value);
            string result = await mes.Content.ReadAsStringAsync();
            var weather = new WeatherParser(result);
            var w = new WeatherInfo(key, value, weather.GetWeathers());
            w.GetWeatherAsJson(w);
        }
    }
}
