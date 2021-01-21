using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace graph.drawer.Views.Render.Converters
{

    class ChartNameToColor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch {
                    string name => RetrieveMap(name),
                    _ => DefaultBrush
            };
        }

        private static SolidColorBrush RetrieveMap(string name)
            => ColorMaps.ContainsKey(name)
                    ? ColorMaps[name]
                    : CreateMap(name);

        private static Random Random { get; } = new Random(1987);

        private static SolidColorBrush CreateMap(string name)
        {
            var brush = ColorMaps.Values.Count != Palette.Count
                    ? Palette.ElementAt(ColorMaps.Values.Count)
                    : DefaultBrush;

            ColorMaps.Add(name, brush);
            return ColorMaps[name];
        }

        private static SolidColorBrush DefaultBrush => new SolidColorBrush(Colors.DimGray);

        private static IDictionary<string, SolidColorBrush> ColorMaps { get; } = new Dictionary<string, SolidColorBrush>();

        private static IReadOnlyCollection<SolidColorBrush> Palette => new[] {
                ColorFromHex("#F3982C"),
                ColorFromHex("#F47A55"),
                ColorFromHex("#008F3C"),
                ColorFromHex("#F2B602"),
                ColorFromHex("#FF5233"),
                ColorFromHex("#0A9DFF"),
                ColorFromHex("#79A31F"),
                ColorFromHex("#05969E"),
                ColorFromHex("#857899")
        };

        private static readonly Func<string, SolidColorBrush> ColorFromHex = hex => (SolidColorBrush) new BrushConverter().ConvertFrom(hex);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }

}
