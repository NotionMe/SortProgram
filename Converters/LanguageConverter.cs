using Practika2_OPAM_Ubohyi_Stanislav.Services;

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