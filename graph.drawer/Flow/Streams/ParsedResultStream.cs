using System;
using System.Collections.Generic;
using Flow.Reactive.Streams.Persisted;
using yaml.parser;

namespace graph.drawer.Flow.Streams
{

    class ParsedResultStream : PersistedStream<ParsedResult>
    {
        public override bool Public => true;

        public override ParsedResult InitialState => new ParsedResult();

    }


    class ParsedResult : PersistedStreamData
    {

        public IEnumerable<Resource> PreInstalls { get; private set; } = new Resource[0];
        public IEnumerable<Resource> Installs { get; private set; } = new Resource[0];
        public IEnumerable<Resource> PostInstalls { get; private set; } = new Resource[0];

        public ParseState State { get; set; } = ParseState.NoFileSelected;

        public void Error(Exception exception)
        {
            PreInstalls = new Resource[0];
            Installs = new Resource[0];
            PostInstalls = new Resource[0];
            State = ParseState.ImpossibleToParse;
        }

        public void Update(IDictionary<Stage, IEnumerable<Resource>> parsed)
        {
            PreInstalls = parsed[Stage.Pre];
            Installs = parsed[Stage.Pre];
            PostInstalls = parsed[Stage.Pre];
            State = ParseState.Parsed;
        }

    }


    internal enum ParseState
    {

        NoFileSelected,
        ImpossibleToParse,
        Parsed

    }

}
