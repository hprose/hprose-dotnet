using System.Collections.Generic;
using CommandLine;

namespace hipig
{
    public class Options
    {
        [Option('r', "reference", Required = true, SetName = "i",
            HelpText = "Imports metadata from the specified assembly")]
        public string Assembly { get; set; }

        [Option('i', "interface", Required = false, Separator = ',', HelpText = "Interfaces name")]
        public IEnumerable<string> InterfacesName { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output file")]
        public string OutputFileName { get; set; }

        [Option('n', "namespace", Required = false, Default = "Hprose.InvocationProxy",
            HelpText = "generate class in namespace")]
        public string NameSpace { get; set; }

        [Option('t', "tail", Required = false, Default = "Proxy", HelpText = "generate class name append the tail")]
        public string ClassTail { get; set; }
    }
}