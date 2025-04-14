using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ReactiveUI;
using Avalonia.Threading;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public class LanguageManager : ReactiveObject
    {
        private static LanguageManager? _instance;
        private Dictionary<string, string> _translations = new();
        private string _currentLanguage = "en";

        public static LanguageManager Instance => _instance ??= new LanguageManager();

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set => this.RaiseAndSetIfChanged(ref _currentLanguage, value);
        }

        public void LoadLanguage(string languageCode)
        {
            try
            {
                // Make sure we're on the UI thread when updating translations
                if (!Dispatcher.UIThread.CheckAccess())
                {
                    Dispatcher.UIThread.Post(() => LoadLanguage(languageCode));
                    return;
                }

                // Fixed path to match the actual file location in the project
                var json = File.ReadAllText($"Styles/Resources/Language/{languageCode}.json");
                _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                CurrentLanguage = languageCode;
                
                // Notify all bindings that translations have changed
                this.RaisePropertyChanged("Item[]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading language {languageCode}: {ex.Message}");
                
                // Fallback to English if there's an error
                if (languageCode != "en")
                {
                    try
                    {
                        LoadLanguage("en");
                    }
                    catch
                    {
                        // If even English fails, create an empty dictionary
                        _translations = new Dictionary<string, string>();
                    }
                }
            }
        }

        public string this[string key] => GetString(key);

        public string GetString(string key)
        {
            return _translations.TryGetValue(key, out var value) ? value : key;
        }
    }
}