using System;
using System.Reactive;
using Flow.Reactive.Services;
using Flow.Reactive.Streams.Ephemeral.Commands;
using graph.drawer.Flow.Streams;
using yaml.parser;

namespace graph.drawer.Flow.Commands
{

    public class UpdateModeCommand : Command
    {

        public ChartMode Mode { get; }

        public UpdateModeCommand(ChartMode mode)
        {
            Mode = mode;
        }

    }


    public class UpdateModeHandler : HandlerNano<UpdateModeCommand>
    {

        public override IObservable<Unit> Connect() 
            => Handle
                   .Update<UpdateModeCommand,ParserConfiguration>(this,(cmd,configuration) => configuration.SelectedMode = cmd.Mode);

    }

}
