using Avalonia.Data.Converters;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using System;
using System.Globalization;

namespace Practika2_OPAM_Ubohyi_Stanislav.Converters
{
    public class LanguageConverter 
    {
        public object? Convert(object? value)
        {
            if (value is string key)
            {
                return LanguageManager.Instance.GetString(key);
            }
            
            return value;
        }

        
    }
}