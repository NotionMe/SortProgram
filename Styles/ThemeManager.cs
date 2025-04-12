using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Styles
{
    public static class ThemeManager
    {
        private static bool isDarkTheme = false;
        
        // Властивість для отримання поточного стану теми
        public static bool IsDarkTheme => isDarkTheme;
        
        // Метод для перемикання теми
        public static void ToggleTheme(Grid mainContentGrid)
        {
            isDarkTheme = !isDarkTheme;
            ApplyTheme(mainContentGrid);
        }
        
        // Встановлення темної теми
        public static void SetDarkTheme(Grid mainContentGrid)
        {
            isDarkTheme = true;
            ApplyTheme(mainContentGrid);
        }
        
        // Встановлення світлої теми
        public static void SetLightTheme(Grid mainContentGrid)
        {
            isDarkTheme = false;
            ApplyTheme(mainContentGrid);
        }
        
        // Застосування теми до елементів інтерфейсу
        private static void ApplyTheme(Grid mainContentGrid)
        {
            if (mainContentGrid == null) return;
            
            // Видаляємо обидва класи теми
            mainContentGrid.Classes.Remove("ThemeLight");
            mainContentGrid.Classes.Remove("ThemeDark");
            
            // Додаємо відповідний клас теми
            if (isDarkTheme)
            {
                mainContentGrid.Classes.Add("ThemeDark");
                UpdateDynamicResources(true);
            }
            else
            {
                mainContentGrid.Classes.Add("ThemeLight");
                UpdateDynamicResources(false);
            }
        }

        // Оновлення динамічних ресурсів
        private static void UpdateDynamicResources(bool isDark)
        {
            var application = Application.Current;
            if (application == null) return;
            
            var resources = application.Resources;
            if (resources == null) return;
            
            if (isDark)
            {
                resources["ThemeBackgroundBrush"] = resources["DarkBackgroundBrush"];
                resources["ThemeForegroundBrush"] = resources["DarkTextBrush"];
                resources["ThemeCardBackgroundBrush"] = resources["DarkCardBackgroundBrush"];
                resources["ThemeBorderBrush"] = resources["DarkBorderColorBrush"];
                resources["ThemePageNumberBrush"] = resources["DarkPageNumberBrush"];
            }
            else
            {
                resources["ThemeBackgroundBrush"] = resources["LightBackgroundBrush"];
                resources["ThemeForegroundBrush"] = resources["LightTextBrush"];
                resources["ThemeCardBackgroundBrush"] = resources["LightCardBackgroundBrush"];
                resources["ThemeBorderBrush"] = resources["LightBorderColorBrush"];
                resources["ThemePageNumberBrush"] = resources["LightPageNumberBrush"];
            }
        }
    }
} 