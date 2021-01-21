using System;
using System.Collections.Generic;
using Flow.Reactive.Streams.Persisted;
using yaml.parser;

namespace graph.drawer.Flow.Streams
{

    class ParserConfigurationsStream : PersistedStream<ParserConfiguration>
    {

        public override ParserConfiguration InitialState => new ParserConfiguration();

    }


    class ParserConfiguration : PersistedStreamData
    {

        public ChartMode SelectedMode { get; set; }

        public IEnumerable<ChartMode> Modes => Enum.GetValues<ChartMode>();

        public ICollection<string> SelectedFilters { get; set; } = new List<string>();

        public IReadOnlyCollection<string> Filters { get; set; } = new List<string>();

        public ParserConfiguration()
        {
            SelectedMode = ChartMode.Install;
        }
    }

}
