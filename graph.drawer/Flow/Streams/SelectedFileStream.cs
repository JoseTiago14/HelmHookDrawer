using System.IO;
using Flow.Reactive.Streams.Persisted;

namespace graph.drawer.Flow.Streams
{

    class SelectedFileStream : PersistedStream<SelectedFile>
    {

        public override bool Public => true;

        public override SelectedFile InitialState => new SelectedFile {
                Path = default
        };

    }


    class SelectedFile : PersistedStreamData
    {

        public FileInfo Path { get; set; }

    }

}
