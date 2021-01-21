namespace graph.drawer.ViewModels
{

    public class MainViewModel
    {

        public FileSelectionViewModel FileSelection { get; }
        public PreviewViewModel Preview { get; }
        public VisualizationViewModel Visualization { get; }

        public MainViewModel(FileSelectionViewModel fileSelection, PreviewViewModel preview, VisualizationViewModel visualization)
        {
            FileSelection = fileSelection;
            Preview = preview;
            Visualization = visualization;
        }

    }

}
