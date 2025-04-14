using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Avalonia.Threading;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        public class Language
        {
            public string Code { get; }
            public string Name { get; }
            public string ResourceKey { get; }

            public Language(string code, string name, string resourceKey)
            {
                Code = code;
                Name = name;
                ResourceKey = resourceKey;
            }
        }
        
        private Language _selectedLanguage;
        
        // Expose the LanguageManager to the view for binding
        public LanguageManager LanguageManager => LanguageManager.Instance;

        public SettingsViewModel()
        {
            AvailableLanguages = new ObservableCollection<Language>
            {
                new Language("en", "English", "Language_English"),
                new Language("uk", "Українська", "Language_Ukrainian")
            };

            // Set the selected language based on the current language
            _selectedLanguage = AvailableLanguages.FirstOrDefault(
                l => l.Code == LanguageManager.CurrentLanguage) ?? AvailableLanguages[0];
        }

        public ObservableCollection<Language> AvailableLanguages { get; }

        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedLanguage, value);
                
                if (value != null && value.Code != LanguageManager.CurrentLanguage)
                {
                    Dispatcher.UIThread.Post(() => 
                    {
                        LanguageManager.LoadLanguage(value.Code);
                        this.RaisePropertyChanged(nameof(LanguageManager));
                    });
                }
            }
        }
    }
}