using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleParserGis
{
    internal class temperature
    {
        internal string max_temp { get; set; }
        internal string min_temp { get; set; }
    }
    internal class WeatherParser : ParserSettings
    {
        List<WeatherItem> weather_items;
        List<string> data_storage;
        List<temperature> temperature_storage;
        string content_html { get; }
        internal WeatherParser(string content_html)
        {
            weather_items = new List<WeatherItem>();
            this.content_html = content_html;
        }
        private IEnumerable<IElement> get_frame(string html_content,string class_name, string selector_name, string searching_class)
        {
            IHtmlDocument angle = new HtmlParser().ParseDocument(html_content);
            IEnumerable<IElement> els = angle.GetElementsByClassName("forecast_frame")
                                        .Where(a => a.QuerySelectorAll("div")
                                        .HasClass("widget__row widget__row_date"));
            return els;
        }
        internal override List<string> GetDays(string html_content)
        {
            data_storage = new List<string>();
            
            foreach (var item in get_frame(html_content, "forecast_frame", "div", "widget__row widget__row_date"))
            {
                foreach (var selector in item.GetElementsByClassName("w_date__day"))
                {
                    data_storage.Add(selector.InnerHtml.Trim() + " " + selector.NextElementSibling.InnerHtml.Trim() + " ");
                }

            }
            return data_storage;
        }
        internal override List<temperature> GetTemperature(string html_content)
        {
            temperature_storage = new List<temperature>();
            foreach (var item in get_frame(html_content, "forecast_frame", "div", "widget__row widget__row_table widget__row_temperature"))
            {
                foreach (var selector in item.GetElementsByClassName("value "))
                {
                    string max = "";
                    string min = "";
                    foreach(var span in selector.GetElementsByClassName("maxt"))
                    {
                        max = span.GetElementsByClassName("unit unit_temperature_c")[0].InnerHtml.Trim();
                    }
                    foreach (var span in selector.GetElementsByClassName("mint"))
                    {
                        min = span.GetElementsByClassName("unit unit_temperature_c")[0].InnerHtml.Trim();
                    }
                    temperature_storage.Add(new temperature { max_temp = max, min_temp = min });
                }

            }

            return temperature_storage;
        }
        internal override List<string> GetWindSpeed(string html_content)
        {
            data_storage = new List<string>();
            foreach (var item in get_frame(html_content, "forecast_frame", "div", "widget__row widget__row_table widget__row_wind-or-gust"))
            {
                foreach (var selector in item.GetElementsByClassName("widget__item"))
                {
                    foreach (var span in selector.GetElementsByClassName("unit unit_wind_m_s"))
                    {
                        data_storage.Add(span.InnerHtml.Trim());
                    }
                }

            }

            return data_storage;
        }
        internal override List<string> GetPrecipitation(string html_content)
        {
            data_storage = new List<string>();
            foreach (var item in get_frame(html_content, "forecast_frame", "div", "widget__row widget__row_table widget__row_precipitation"))
            {
                foreach (var selector in item.GetElementsByClassName("widget__item"))
                {
                    foreach (var span in selector.GetElementsByClassName("w_prec__value"))
                    {
                        data_storage.Add(span.InnerHtml.Trim());
                    }
                }
            }

            return data_storage;
        }
        internal override string GetState(string html_content)
        {
            return "";
        }
        internal List<WeatherItem> GetWeathers()
        {
            int list_length = GetDays(content_html).Count();
           for(int i = 0; i < list_length; i++)
            {
                temperature t = GetTemperature(content_html)[i];
                weather_items.Add(new WeatherItem
                {
                    Day = GetDays(content_html)[i],
                    MaxTemperature = t.max_temp,
                    MinTemperature = t.min_temp,
                    WindSpeed = GetWindSpeed(content_html)[i],
                    Precipitation = GetPrecipitation(content_html)[i]
                }) ;
            }
            return weather_items;
        }
    }
}
