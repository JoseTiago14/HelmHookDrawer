using System;
using System.Globalization;
using System.Windows.Data;

namespace graph.drawer.Views.Render.Converters
{

    class ChartNameDefaultBoxText : IValueConverter
    {

        private const string DefaultText = "default";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch {
                    string chartName => chartName == string.Empty ? DefaultText : chartName,
                    _ => value
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch {
                    string chartName => chartName == DefaultText ? string.Empty : chartName,
                    _ => value
            };
        }

    }

}
