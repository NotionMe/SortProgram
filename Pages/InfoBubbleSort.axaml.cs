using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
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
            var mainWindow = this.VisualRoot as SortProgram;
            
            // Якщо головне вікно доступне, повертаємося до сторінки зі списком алгоритмів
            if (mainWindow != null)
            {
                // Створюємо новий екземпляр сторінки SortingAlgorithmsPage
                var algorithmsPage = new SortingAlgorithmsPage();
                
                // Переходимо до цієї сторінки
                mainWindow.NavigateToPagePublic(algorithmsPage);
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