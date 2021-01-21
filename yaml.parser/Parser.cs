using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace yaml.parser
{
    public abstract class Parser<T>
    {
        private readonly YamlReader _reader;

        public T Parse(string yaml, ChartMode mode) => Order(_reader.Read(yaml), mode);

        protected abstract T Order(IReadOnlyCollection<Resource> items, ChartMode chartMode);

        protected Parser(YamlReader reader)
        {
            _reader = reader;
        }
    }

    public enum ChartMode
    {
        Install,
        Upgrade,
        Delete,
        Rollback
    }

    public class ListParser : Parser<IEnumerable<Resource>>
    {
        public ListParser(YamlReader reader) : base(reader) { }

        protected override IEnumerable<Resource> Order(IReadOnlyCollection<Resource> items, ChartMode chartMode)
        {
            Predicate<Resource> pred = chartMode switch
            {
                ChartMode.Install => r => r.IsInstallHook(),
                ChartMode.Upgrade => r => r.IsUpgradeHook(),
                ChartMode.Delete => r => r.IsDeleteHook(),
                ChartMode.Rollback => r => r.IsRollbackHook(),
                _=> throw new ArgumentOutOfRangeException()
            };

            var pre = items.Where(r=>r.IsPreHook()&& pred(r)).OrderBy(r=> (r.Weight,r.Name));
            var current = items.Where(r => r.HasNoHook()); //TODO - Confirm order between different types
            var post = items.Where(r => r.IsPostHook() && pred(r)).OrderBy(r => (r.Weight, r.Name));

            return pre.Concat(current).Concat(post);
        } 
    }

    public class TreeParser : Parser<IEnumerable<Resource>>
    {
        public TreeParser(YamlReader reader) : base(reader) { }

        protected override IEnumerable<Resource> Order(IReadOnlyCollection<Resource> items, ChartMode chartMode) => items.Reverse(); //TODO
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