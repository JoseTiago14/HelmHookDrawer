using System.Net.Mime;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Flow.Wpf;
using graph.drawer.ViewModels;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (Current.MainWindow != null)
                Current.MainWindow.Title = "Hook Drawer";
        }


        private Assembly ThisAssembly { get; }
        private CaliburnHost<MainViewModel> Host { get; }

    }

}
