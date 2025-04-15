using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Practika2_OPAM_Ubohyi_Stanislav.Styles;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class SettingsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public class Language
        {
            public string Code { get; }
            public string Name { get; }

            public Language(string code, string name)
            {
                Code = code;
                Name = name;
            }
        }

        private readonly LanguageManager _languageManager;
        private bool _isDarkTheme;
        private Grid _mainContentGrid;
        private Language _selectedLanguage;

        public SettingsViewModel()
        {
            _languageManager = LanguageManager.Instance;
            _isDarkTheme = ThemeManager.IsDarkTheme;

            AvailableLanguages = new ObservableCollection<Language>
            {
                new Language("en", "English"),
                new Language("uk", "Українська")
            };

            // Set the selected language based on the current language
            _selectedLanguage = AvailableLanguages.FirstOrDefault(
                l => l.Code == LanguageManager.CurrentLanguage) ?? AvailableLanguages[0];
        }

        public LanguageManager LanguageManager => _languageManager;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    if (_mainContentGrid != null)
                    {
                        if (_isDarkTheme)
                        {
                            ThemeManager.SetDarkTheme(_mainContentGrid);
                        }
                        else
                        {
                            ThemeManager.SetLightTheme(_mainContentGrid);
                        }
                    }
                    OnPropertyChanged(nameof(IsDarkTheme));
                }
            }
        }

        

        public void Initialize(Grid mainContentGrid)
        {
            _mainContentGrid = mainContentGrid;
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
                        // Apply the language change
                        LanguageManager.SetLanguage(value.Code);

                        // Force update to immediately refresh all UI elements with new translations
                        LanguageManager.ForceUpdate();

                        // Update any ViewModel properties that depend on LanguageManager
                        this.RaisePropertyChanged(nameof(LanguageManager));
                    });
                }
            }
        }
    }
}