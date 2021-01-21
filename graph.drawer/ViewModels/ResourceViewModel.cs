using System.Collections.Generic;
using System.Linq;
using yaml.parser;

namespace graph.drawer.ViewModels
{

    public class ResourceViewModel
    {

        public ICollection<HookType> Hooks { get; set; }
        public string Namespace { get; set; }
        public long Weight { get; set; }
        public string Name { get; set; }
        public string ChartName { get; set; }
        public KindType Kind { get; set; }

        //render props
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public bool IsNewLine { get; set; }
        public bool IsToRenderArrow { get; set; }

        public ResourceViewModel(Resource resource, IList<Resource> allResources, int columns, bool isSequential)
        {
            Kind = resource.Kind;
            Name = resource.Name;
            ChartName = resource.ChartName;
            Weight = resource.Weight;
            Namespace = resource.Namespace;
            Hooks = resource.Hooks;
            IsFirst = resource == allResources.First();
            IsLast = resource == allResources.Last();
            IsNewLine = allResources.Where(r => allResources.IndexOf(resource) % columns == 0)
                                    .Any(r => r == resource);
            IsToRenderArrow = isSequential;
        }

    }

}
