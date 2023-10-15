using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Client.Presentation.Converters;

/// <summary>
/// Bool To Visibility
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// true일때 값
    /// </summary>
    public Visibility TrueVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// false일때 값
    /// </summary>
    public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool invert = false;

        if(parameter != null)
        {
            invert = Boolean.Parse(parameter.ToString()!);
        }
        if (value is bool boolValue)
        {
            return ((boolValue && !invert) || (!boolValue && invert)) ? TrueVisibility : FalseVisibility;
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}