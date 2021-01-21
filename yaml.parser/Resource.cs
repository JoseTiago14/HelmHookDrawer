using System.Collections.Generic;
using System.Linq;

namespace yaml.parser
{
    public class Resource
    {
        public Resource(KindType kind, string name, string ns, long weight, string chartName, ICollection<HookType> hooks)
        {
            Kind = kind;
            Name = name;
            Namespace = ns;
            Weight = weight;
            ChartName = chartName;
            Hooks = hooks;
        }

        public KindType Kind { get; }
        public string Name { get; }
        public string Namespace { get; }
        public long Weight { get; }
        public string ChartName { get; }
        public ICollection<HookType> Hooks { get; set; }
    }

    public enum KindType
    {
        Secret,
        Service,
        Deployment,
        Job,
        ExternalService,
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

    public static class ResourceExtension
    {
        public static bool HasNoHook(this Resource resource) =>
            !(IsPreHook(resource)||IsPostHook(resource));

        public static bool IsPreHook(this Resource resource) =>
            resource.Hooks.Any(h => h switch
            {
                HookType.PreInstall => true,
                HookType.PreDelete => true,
                HookType.PreUpgrade => true,
                HookType.PreRollback => true,
                _ => false,
            });

        public static bool IsPostHook(this Resource resource) =>
            resource.Hooks.Any(h => h switch
            {
                HookType.PostInstall => true,
                HookType.PostDelete => true,
                HookType.PostUpgrade => true,
                HookType.PostRollback => true,
                _ => false,
            });

        public static bool IsInstallHook(this Resource resource) =>
            resource.Hooks.Any(h => h switch
            {
                HookType.PreInstall => true,
                HookType.PostInstall => true,
                _ => false,
            });

        public static bool IsDeleteHook(this Resource resource) =>
            resource.Hooks.Any(h => h switch
            {
                HookType.PreDelete => true,
                HookType.PostDelete => true,
                _ => false,
            });

        public static bool IsUpgradeHook(this Resource resource) =>
            resource.Hooks.Any(h => h switch
            {
                HookType.PreUpgrade => true,
                HookType.PostUpgrade => true,
                _ => false,
            });

        public static bool IsRollbackHook(this Resource resource) =>
            resource.Hooks.Any(h => h switch
            {
                HookType.PreRollback => true,
                HookType.PostRollback => true,
                _ => false,
            });
    }
}