using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info
{
    public partial class InfoBubbleSort : UserControl
    {
        public InfoBubbleSort()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        // Обробник натискання на кнопку "Назад"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо доступ до головного вікна
            var mainWindow = this.VisualRoot as Practika2_OPAM_Ubohyi_Stanislav.SortProgram;
            
            // Якщо головне вікно доступне, повертаємося до сторінки зі списком алгоритмів
            if (mainWindow != null)
            {
                // Створюємо новий екземпляр сторінки SortingAlgorithmsPage
                var algorithmsPage = new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage();
                
                // Переходимо до цієї сторінки
                mainWindow.NavigateToPagePublic(algorithmsPage);
            }
            else
            {
                // Альтернативний метод навігації, якщо головне вікно недоступне
                if (this.Parent is ContentControl contentControl)
                {
                    contentControl.Content = new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage();
                }
            }
        }
        
        // Обробник натискання на кнопку "Вперед"
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Тут можна було б реалізувати навігацію до наступної статті
            // Для прикладу просто повертаємося до сторінки зі списком алгоритмів
            BackButton_Click(sender, e);
        }
    }
} 