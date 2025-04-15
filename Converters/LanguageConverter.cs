using Avalonia.Data.Converters;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using System;
using System.Globalization;

namespace Practika2_OPAM_Ubohyi_Stanislav.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string key)
            {
                return LanguageManager.Instance.GetString(key);
            }
            
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // ConvertBack is not supported for localization
            throw new NotImplementedException();
        }
    }
}