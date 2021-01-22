using CommandLine;
using yaml.parser;

namespace console.runner
{
    public class Options
    {
        [Option("mode", Default = ChartMode.Install, Required = false)]
        public ChartMode Mode { get; set; }
        [Option("path", Required = true)]
        public string Path { get; set; }
    }
}