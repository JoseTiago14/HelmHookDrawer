using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using Flow.Reactive.Services;
using Flow.Reactive.Streams.Ephemeral.Commands;
using graph.drawer.Flow.Streams;
using Reactive.Bindings.Extensions;

namespace graph.drawer.Flow.Commands
{

    class ReplaceFileCommand : Command { }


    class ReplaceFileHandler : HandlerNano<ReplaceFileCommand>
    {

        public override IObservable<Unit> Connect()
            => Handle
              .ObserveOnDispatcher()
              .Select(_ => FilePicker.ShowDialog())
              .Where(dialog => dialog == DialogResult.OK)
              .Update<DialogResult, SelectedFile>(this, (result, file) => file.Path = new FileInfo(FilePicker.FileName));

        public ReplaceFileHandler()
        {
            FilePicker = new OpenFileDialog {
                    DefaultExt = "yaml",
                    Filter = "helm templates (*.yaml, *.yml)|*.yaml;*.yml",
                    RestoreDirectory = true,
                    Title = "Load helm template",
                    Multiselect = false,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    ShowReadOnly = false,
                    ReadOnlyChecked = false
            };
        }

        public OpenFileDialog FilePicker { get; }

    }

}
