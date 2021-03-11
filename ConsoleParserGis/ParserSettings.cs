using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParserGis
{
    internal class ParserSettings : ISettings<Dictionary<string,string>>
    {
        public string URL { get; }

        public string SUFFIX { get; }

        public Dictionary<string,string> Citiesrefs { get; }

        public string MAIN_CONTENT_TAG { get; }
        private string searching_atr { get; }
        private string city_atr { get; }
        private string selector { get; }

        internal HttpClient client;
        internal ParserSettings(string url, 
                                string suffix,
                                string main_content_tag,
                                string searching_atr,
                                string city_atr, 
                                string selector)
        {
            URL = url;
            SUFFIX = suffix;
            MAIN_CONTENT_TAG = main_content_tag;
            this.searching_atr = searching_atr;
            this.city_atr = city_atr;
            this.selector = selector;
            client = new HttpClient();
            Citiesrefs = new Dictionary<string, string>();
        }
        internal ParserSettings() { }

        public async Task<Dictionary<string,string>> GetCitiesRefrs()
        {
            HttpResponseMessage mes = await client.GetAsync(URL);
            string result = await mes.Content.ReadAsStringAsync();
            IHtmlDocument angle = new HtmlParser().ParseDocument(result);

            foreach (var el in angle.QuerySelectorAll(MAIN_CONTENT_TAG))
            {
                var anhors = el.QuerySelectorAll(selector);
                foreach (var i in anhors) Citiesrefs.Add(i.GetAttribute(city_atr),
                                                         URL + i.GetAttribute(searching_atr) + SUFFIX);
            }
            return Citiesrefs; 
        }

        internal virtual List<string> GetDays(string html_content)
        {
            return new List<string>();
        }
        internal virtual List<string> GetTemperature(string html_content)
        {
            return new List<string>();
        }
        internal virtual List<string> GetWindSpeed(string html_content)
        {
            return new List<string>();
        }
        internal virtual List<string> GetPrecipitation(string html_content)
        {
            return new List<string>();
        }
        internal virtual string GetHumidity(string html_content)
        {
            return "";
        }
        internal virtual string GetState(string html_content)
        {
            return "";
        }

    }
}
