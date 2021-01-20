using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace yaml.parser
{
    public abstract class Parser<T>
    {
        private readonly YamlReader _reader;

        public T Parse(string yaml) => Order(_reader.Read(yaml));

        protected abstract T Order(IEnumerable<Resource> items);

        protected Parser(YamlReader reader)
        {
            _reader = reader;
        }
    }

    public class ListParser : Parser<IEnumerable<Resource>>
    {
        public ListParser(YamlReader reader) : base(reader) { }

        protected override IEnumerable<Resource> Order(IEnumerable<Resource> items) => items; //TODO
    }

    public class TreeParser : Parser<IEnumerable<Resource>>
    {
        public TreeParser(YamlReader reader) : base(reader) { }

        protected override IEnumerable<Resource> Order(IEnumerable<Resource> items) => items.Reverse(); //TODO
    }

    public class ParsersServiceCollection : ServiceCollection
    {
        public ParsersServiceCollection()
        {
            this.AddSingleton(typeof(ILogger), sp => sp.GetService<ILoggerFactory>().CreateLogger(typeof(ParsersServiceCollection)))
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton<YamlReader>()
                .AddSingleton<ListParser>()
                .AddSingleton<TreeParser>();
        }
    }
}