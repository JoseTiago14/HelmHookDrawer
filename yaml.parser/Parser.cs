using System;
using System.Collections.Generic;
using System.Linq;

namespace yaml.parser
{

    public class Parser
    {

        private readonly YamlReader _reader;

        public IDictionary<Stage, IEnumerable<Resource>> Parse(string yaml, ChartMode chartMode)
        {
            var items = _reader.Read(yaml);
            Predicate<Resource> pred = chartMode switch {
                    ChartMode.Install => r => r.IsInstallHook(),
                    ChartMode.Upgrade => r => r.IsUpgradeHook(),
                    ChartMode.Delete => r => r.IsDeleteHook(),
                    ChartMode.Rollback => r => r.IsRollbackHook(),
                    _ => throw new ArgumentOutOfRangeException()
            };

            return new Dictionary<Stage, IEnumerable<Resource>> {
                    {Stage.Pre, items.Where(r => r.IsPreHook() && pred(r)).OrderBy(r => (r.Weight, r.Name))},
                    {Stage.Deploy, items.Where(r => r.HasNoHook())},
                    {Stage.Post, items.Where(r => r.IsPostHook() && pred(r)).OrderBy(r => (r.Weight, r.Name))}
            };
        }

        public Parser(YamlReader reader)
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


    // Ingress
    // Servico
    // Deployment
    // Secrets
    // Config
    // Labels para agrupar visualmente? (ordem!?) Tipo Gatekeeper agrupados etc,,
    public enum Stage
    {

        Pre,
        Deploy,
        Post

    }

}
