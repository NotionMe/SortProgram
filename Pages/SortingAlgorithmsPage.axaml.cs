using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class SortingAlgorithmsPage : UserControl
    {
        public SortingAlgorithmsPage()
        {
            InitializeComponent();
        }
        
        // Обробник події для кнопки "Переглянути демо"
        private void ViewArticleDemo_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо доступ до головного вікна
            var mainWindow = this.VisualRoot as SortProgram;
            
            // Якщо головне вікно доступне, переходимо до сторінки ArticleViewPage
            if (mainWindow != null)
            {
                // Створюємо новий екземпляр сторінки ArticleViewPage
                var articlePage = new ArticleViewPage();
                
                // Переходимо до цієї сторінки
                mainWindow.NavigateToPagePublic(articlePage);
            }
        }
    }
}
