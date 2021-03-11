using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParserGis
{
    interface ISettings<T> where T : Dictionary<string, string>
    {
        string URL { get; }
        string SUFFIX { get; }
        string MAIN_CONTENT_TAG { get; }
        T Citiesrefs { get; }
        Task<T> GetCitiesRefrs();
    }
}
