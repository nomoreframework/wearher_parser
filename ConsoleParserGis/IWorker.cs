using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParserGis
{
    interface IWorker
    {
        ParserSettings settings { get; }
    }
}
