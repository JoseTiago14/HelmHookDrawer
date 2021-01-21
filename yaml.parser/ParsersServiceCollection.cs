using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace yaml.parser
{

    public class ParsersServiceCollection : ServiceCollection
    {
        public ParsersServiceCollection()
        {
            this.AddSingleton(typeof(ILogger), sp => sp.GetService<ILoggerFactory>().CreateLogger(typeof(ParsersServiceCollection)))
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton<YamlReader>()
                .AddSingleton<Parser>();
        }
    }
}
