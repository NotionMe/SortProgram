using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var elementCountComboBox = this.FindControl<ComboBox>("ElementCountComboBox");
            elementCountComboBox!.SelectedIndex = 0 ; // За замовчуванням 10 елементів
        }

        private void ResetProgress(object sender, RoutedEventArgs e)
        {
            // Логіка для скидання прогресу
            // Наприклад, очищення файлу прогресу або даних у пам’яті
        }
    }
}