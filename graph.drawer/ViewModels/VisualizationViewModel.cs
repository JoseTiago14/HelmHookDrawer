using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using Flow.Reactive;
using Flow.Reactive.ReactiveProperty;
using graph.drawer.Flow.Streams;
using Reactive.Bindings;

namespace graph.drawer.ViewModels
{

    public class VisualizationViewModel
    {

        public ReactiveProperty<bool> IsParsed { get; }

        public ReactiveProperty<string> ErrorMessage { get; }
        public ReactiveProperty<Visibility> ErrorMessageVisibility { get; set; }

        public ReactiveProperty<IEnumerable<ResourceViewModel>> PostInstalls { get; set; }
        public ReactiveProperty<IEnumerable<ResourceViewModel>> Installs { get; set; }
        public ReactiveProperty<IEnumerable<ResourceViewModel>> PreInstalls { get; set; }

        public int Columns => 4;


        #region construction

        public VisualizationViewModel(IFlow flow)
        {
            IsParsed = flow.Bind<bool, ParsedResult>(result => result.State == ParseState.Parsed, Disposables);

            ErrorMessage = flow.Bind<string, ParsedResult>(result => ErrorMessages[result.State], Disposables);
            ErrorMessageVisibility = ErrorMessage.Select(msg => msg == default ? Visibility.Collapsed : Visibility.Visible).ToReactiveProperty();

            PreInstalls = flow.Bind<IEnumerable<ResourceViewModel>, ParsedResult>(result => result.PreInstalls.Select(r => new ResourceViewModel(r, result.PreInstalls.ToList(), Columns)), Disposables);
            Installs = flow.Bind<IEnumerable<ResourceViewModel>, ParsedResult>(result => result.Installs.Select(r => new ResourceViewModel(r, result.Installs.ToList(), Columns)), Disposables);
            PostInstalls = flow.Bind<IEnumerable<ResourceViewModel>, ParsedResult>(result => result.PostInstalls.Select(r => new ResourceViewModel(r, result.PostInstalls.ToList(), Columns)), Disposables);
        }

        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        #endregion


        private static Dictionary<ParseState, string> ErrorMessages => new Dictionary<ParseState, string> {
                {ParseState.NoFileSelected, "Please select a file to analyze installation order"},
                {ParseState.ImpossibleToParse, "Impossible to parse selected file, please make sure\nit is a valid helm template"},
                {ParseState.Parsed, default}
        };

    }

}
