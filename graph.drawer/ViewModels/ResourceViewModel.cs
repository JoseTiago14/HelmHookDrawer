using System.Collections.Generic;
using yaml.parser;

namespace graph.drawer.ViewModels
{

    public class ResourceViewModel
    {

        public ICollection<HookType> Hooks { get; set; }

        public string Namespace { get; set; }

        public long Weight { get; set; }

        public string Name { get; set; }

        public KindType Kind { get; set; }

        public ResourceViewModel(Resource resource)
        {
            Kind = resource.Kind;
            Name = resource.ChartName;
            Weight = resource.Weight;
            Namespace = resource.Namespace;
            Hooks = resource.Hooks;
        }

    }

}
