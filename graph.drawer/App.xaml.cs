using System.Reflection;
using Flow.Wpf;
using graph.drawer.ViewModels;
using graph.drawer.Views;

namespace graph.drawer
{

    public partial class App
    {

        public App()
        {
            ThisAssembly = typeof(App).Assembly;
            Host = new CaliburnHost<MainViewModel>(this,
                                                   ("Flow", ThisAssembly));
        }

        private Assembly ThisAssembly { get; }
        private CaliburnHost<MainViewModel> Host { get; }

    }

}
