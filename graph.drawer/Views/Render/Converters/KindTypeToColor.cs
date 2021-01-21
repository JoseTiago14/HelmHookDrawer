using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using yaml.parser;

namespace graph.drawer.Views.Render.Converters
{
    
    class KindTypeToColor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch {
                    KindType kind => ColorMappings[kind],
                    _ => default
            };
        }

        private static IReadOnlyDictionary<KindType, SolidColorBrush> ColorMappings => new Dictionary<KindType, SolidColorBrush> {
                {KindType.Secret, new SolidColorBrush(Colors.OrangeRed)},
                {KindType.Service, new SolidColorBrush(Colors.CornflowerBlue)},
                {KindType.Deployment, new SolidColorBrush(Colors.Goldenrod)},
                {KindType.Ingress, new SolidColorBrush(Colors.ForestGreen)},
                {KindType.Job, new SolidColorBrush(Colors.Aqua)},
                {KindType.ConfigMap, new SolidColorBrush(Colors.MediumPurple)},
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }

}
