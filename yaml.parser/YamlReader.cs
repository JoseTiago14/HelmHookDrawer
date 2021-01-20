using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace yaml.parser
{

    public class YamlReader
    {

        public IEnumerable<Item> Read(string yaml)
        {
            return new Item[] {
                    new Item() {Kind = Kind.Service},
                    new Item() {Kind = Kind.Service},
                    new Item() {Kind = Kind.ExternalService},
                    new Item() {Kind = Kind.Deployment},
                    new Item() {Kind = Kind.Ingress},
                    new Item() {Kind = Kind.Job},
                    new Item() {Kind = Kind.Deployment},
            };
        } // TODO

    }


    public class Item
    {

        public Kind Kind { get; set; }

    }


    public enum Kind
    {

        Service,
        ExternalService,
        Deployment,
        Ingress,
        Job

    }


    public abstract class Parser<T>
    {

        private readonly YamlReader _reader;

        public T Parse(string yaml) => Order(_reader.Read(yaml));

        protected abstract T Order(IEnumerable<Item> items);

        protected Parser(YamlReader reader)
        {
            _reader = reader;
        }

    }


    public class ListParser : Parser<IEnumerable<Item>>
    {

        public ListParser(YamlReader reader) : base(reader) { }

        protected override IEnumerable<Item> Order(IEnumerable<Item> items) => items; //TODO

    }


    public class TreeParser : Parser<IEnumerable<Item>>
    {

        public TreeParser(YamlReader reader) : base(reader) { }

        protected override IEnumerable<Item> Order(IEnumerable<Item> items) => items.Reverse(); //TODO

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
