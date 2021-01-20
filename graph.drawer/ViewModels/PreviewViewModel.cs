using System.IO;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Flow.Reactive;
using Flow.Reactive.ReactiveProperty;
using graph.drawer.Flow.Streams;
using Reactive.Bindings;

namespace graph.drawer.ViewModels
{

    public class PreviewViewModel
    {

        public ReactiveProperty<string> Preview { get; }

        public ReactiveProperty<HorizontalAlignment> PreviewAlignment { get; }


        #region construction

        public PreviewViewModel(IFlow flow)
        {
            Preview = flow.Bind<string, SelectedFile>(file => file.Path == default
                                                              ? NoFilePreview
                                                              : File.ReadAllText(file.Path.FullName),
                                                      Disposables);

            PreviewAlignment = flow.Bind<HorizontalAlignment, SelectedFile>(file => file.Path == default
                                                                                    ? HorizontalAlignment.Center
                                                                                    : HorizontalAlignment.Left,
                                                                            Disposables);
        }

        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        #endregion


        private static string NoFilePreview => "Select a file to see its preview";

    }

}
