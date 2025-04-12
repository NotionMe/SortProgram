using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info
{
    public partial class ComparisonPage : UserControl
    {
        public ComparisonPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Спочатку спробуємо отримати доступ до головного вікна
            var mainWindow = this.VisualRoot as Practika2_OPAM_Ubohyi_Stanislav.SortProgram;
            if (mainWindow != null)
            {
                // Якщо головне вікно доступне, використовуємо його метод навігації
                mainWindow.NavigateToPagePublic(new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage());
            }
            else if (this.Parent is ContentControl contentControl)
            {
                // Альтернативний метод навігації
                contentControl.Content = new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage();
            }
        }
    }
} 