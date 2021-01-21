using System.IO;
using System.Reactive.Disposables;
using Flow.Reactive;
using Flow.Reactive.ReactiveProperty;
using graph.drawer.Flow.Commands;
using graph.drawer.Flow.Streams;
using Reactive.Bindings;

namespace graph.drawer.ViewModels
{

    public class FileSelectionViewModel
    {

        public ReactiveProperty<string> FilePlaceholder { get; }

        public ReactiveCommand ReplaceFile { get; }


        #region construction

        public FileSelectionViewModel(IFlow flow)
        {
            FilePlaceholder = flow.Bind<string, SelectedFile>(file => file.Path?.FullName ?? NoFileSelectedPlaceholder,
                                                              Disposables);
            ReplaceFile = flow.BindCommand(new ReplaceFileCommand(), Disposables);
        }

        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        #endregion


        private static string NoFileSelectedPlaceholder => "No file selected please click the button to select a new file";
        private static FileInfo FileNotSelected => new FileInfo("file_not_selected");

    }

}
