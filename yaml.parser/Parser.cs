using System;
using System.Collections.Generic;
using System.Linq;

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

    public class TreeParser : Parser<Tree>
    {
        public TreeParser(YamlReader reader) : base(reader) { }

        protected override Tree Order(IReadOnlyCollection<Resource> items, ChartMode chartMode)
        {
            var tree = new Tree() {
                    //{Phase.Pre, IEnumerable<Resource>}, //seq
                    //{Phase.Current, y}, //par
                    //{Phase.Post, IEnumerable<Resource>},//seq
            };

            return tree;
        }

    }


    public class Tree : Dictionary<Phase, IEnumerable<Resource>>
    {


        public Tree()
        {
            
        }


    }


    public class TreeItem
    {

        public Resource Resource { get; }

        public TreeItem(Resource resource)
        {
            Resource = resource;
        }
    }


    public enum Phase
    {
        Pre,
        Current,
        Post
    }

}