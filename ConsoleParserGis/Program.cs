using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using System.Threading;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Diagnostics;

namespace ConsoleParserGis
{
   public class Program
    {
        const string URL = "https://www.gismeteo.ru";
        const string suffix = "10-days";
        static Dictionary<string, string> dict;
        static ParserSettings obj;
        static void Main(string[] args)
        {
            { 

            }
        }

       async static Task<string> get_data_synch()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            obj = new ParserSettings(URL, suffix, "noscript", "href", "data-name", "a");

            dict = await obj.GetCitiesRefrs();
            new Worker().ParseSynchronus(dict, obj);
            s.Stop();
            Print("synch time = " + s.ElapsedMilliseconds.ToString());
            return "";
        }
         async static Task<string> get_data_parallel()
        {
            obj = new ParserSettings(URL, suffix, "noscript", "href", "data-name", "a");
            dict = await obj.GetCitiesRefrs();
            new Worker().ParseParallel(dict, obj);
            return "";
        }
      internal  static void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message + "\n" + "-----------------------------------------" + "\n");
            Console.ResetColor();
        }
       internal static void PrintErr(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message + "\n" + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + "\n");
            Console.ResetColor();
        }
    }
    internal class Storage
    {
        internal ParserSettings settings { get; }
        internal string key { get; }
        internal string value { get; }
    }
}
