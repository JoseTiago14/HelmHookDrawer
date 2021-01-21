using Flow.Wpf.Caliburn;
using graph.drawer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace graph.drawer
{

    public class GraphDrawerServiceCollection : ServiceCollection
    {

        public GraphDrawerServiceCollection()
        {
            this.AddSingleton<ServiceProviderBootstrapper<MainViewModel>>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<PreviewViewModel>()
                .AddSingleton<VisualizationViewModel>()
                .AddSingleton<FileSelectionViewModel>();
        }

    }

}
