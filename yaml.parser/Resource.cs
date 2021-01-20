using System.Collections.Generic;

namespace yaml.parser
{
    public class Resource
    {
        public Resource(KindType kind, string name, string ns, long weight, ICollection<HookType> hooks)
        {
            Kind = kind;
            Name = name;
            Namespace = ns;
            Weight = weight;
            Hooks = hooks;
        }
        public KindType Kind { get; }
        public string Name { get; }
        public string Namespace { get; }
        public long Weight { get; }
        public ICollection<HookType> Hooks { get; set; }
    }

    public enum KindType
    {
        Service,
        ExternalService,
        Deployment,
        Ingress,
        Job
    }

    public enum HookType
    {
        PreInstall,
        PostInstall,
        PreDelete,
        PostDelete,
        PreUpgrade,
        PostUpgrade,
        PreRollback,
        PostRollback
    }
}