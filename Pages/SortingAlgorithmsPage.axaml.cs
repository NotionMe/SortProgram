using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Info;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class SortingAlgorithmsPage : UserControl
    {
        public SortingAlgorithmsPage()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        // Обробник події для відкриття інформаційної сторінки про Bubble Sort
        private void InfoBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо доступ до головного вікна
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoBubbleSort());
            }
        }


        // Обробник події для відкриття інтерактивної візуалізації Bubble Sort
        private void GoToTryIt_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо доступ до головного вікна
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new BubbleSort());
            }
        }

        
    }
}
