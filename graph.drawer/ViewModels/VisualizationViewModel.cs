using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows;
using Flow.Reactive;
using Flow.Reactive.ReactiveProperty;
using Flow.Rx.Extensions;
using graph.drawer.Flow.Commands;
using graph.drawer.Flow.Streams;
using Reactive.Bindings;
using yaml.parser;

namespace graph.drawer.ViewModels
{

    public class VisualizationViewModel
    {

        public ReactiveProperty<bool> IsParsed { get; }

        public ReactiveProperty<string> ErrorMessage { get; }
        public ReactiveProperty<Visibility> ErrorMessageVisibility { get; set; }

        public ReactiveProperty<IEnumerable<ResourceViewModel>> PostInstalls { get; }
        public ReactiveProperty<IEnumerable<ResourceViewModel>> Installs { get; }
        public ReactiveProperty<IEnumerable<ResourceViewModel>> PreInstalls { get; }

        public ReactiveProperty<IEnumerable<CheckBoxItem>> Checkboxes { get; }

        public ReactiveProperty<ChartMode> SelectedMode { get; }
        public IEnumerable<ChartMode> Modes => Enum.GetValues<ChartMode>();

        public int Columns => 4;


        #region construction

        public VisualizationViewModel(IFlow flow)
        {
            IsParsed = flow.Bind<bool, ParsedResult>(result => result.State == ParseState.Parsed, Disposables);

            ErrorMessage = flow.Bind<string, ParsedResult>(result => ErrorMessages[result.State], Disposables);
            ErrorMessageVisibility = ErrorMessage.Select(msg => msg == default ? Visibility.Collapsed : Visibility.Visible).ToReactiveProperty();

            Checkboxes = flow.Bind<IEnumerable<CheckBoxItem>, ParsedResult>(result => {
                                                                                var results = result.All
                                                                                                    .Select(r => r.ChartName)
                                                                                                    .OrderBy(name => name)
                                                                                                    .Distinct()
                                                                                                    .Select(chartName => new CheckBoxItem(chartName, flow))
                                                                                                    .ToList();
                                                                                return results;
                                                                            },
                                                                            Disposables);

            ReactiveProperty<IEnumerable<ResourceViewModel>> bind_with_filters(Func<ParsedResult, IEnumerable<Resource>> func, bool isSequential)
                => flow.Bind<
                        IEnumerable<ResourceViewModel>,
                        ParsedResult
                >(result => Checkboxes
                           .Select(cb => {
                                var checkboxes = cb.ToList();
                                var trigger = checkboxes.Select(c => c.Checked)
                                                        .Merge();

                                return trigger.Select(_ => {
                                    var charts = checkboxes.Select(cb => (isChecked: cb.Checked.Value, name: cb.ChartName))
                                                           .Where(chart => chart.isChecked)
                                                           .Select(chart => chart.name);

                                    return func(result)
                                          .Where(r => charts.Contains(r.ChartName))
                                          .Select(r => new ResourceViewModel(r, func(result).ToList(), Columns, isSequential));
                                });
                            })
                           .Concat(),
                  Disposables);

            PreInstalls = bind_with_filters(result => result.PreInstalls, true);
            Installs = bind_with_filters(result => result.Installs, false);
            PostInstalls = bind_with_filters(result => result.PostInstalls, true);

            SelectedMode = Observable.Return(Modes.First()).ToReactiveProperty();

            SelectedMode.Select(mode => flow.Send(new UpdateModeCommand(mode)))
                        .Concat()
                        .Subscribe()
                        .AddToDisposables(Disposables);
        }

        private CompositeDisposable Disposables { get; } = new CompositeDisposable();

        #endregion


        private static Dictionary<ParseState, string> ErrorMessages => new Dictionary<ParseState, string> {
                {ParseState.NoFileSelected, "Please select a file to analyze installation order"},
                {ParseState.ImpossibleToParse, "Impossible to parse selected file, please make sure\nit is a valid helm template"},
                {ParseState.Parsed, default}
        };

    }


    public class CheckBoxItem
    {

        public string ChartName { get; set; }

        public ReactiveProperty<bool> Checked { get; set; }

        public CheckBoxItem(string name, IFlow flow)
        {
            ChartName = name;
            Checked = Observable.Return(true).ToReactiveProperty();
        }

    }

}
