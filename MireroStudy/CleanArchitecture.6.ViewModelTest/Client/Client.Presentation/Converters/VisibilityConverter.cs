using System;
using System.Globalization;
using System.Windows.Data;
using WindowsVisibility = System.Windows.Visibility;

namespace Client.Presentation.Converters;

/// <summary>
/// Windows.Visibility To Domain.Visibility
/// </summary>
public class VisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var source = (byte)value;
        var result = (WindowsVisibility)Enum.Parse(typeof(WindowsVisibility), source.ToString());
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}