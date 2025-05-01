using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.InfoAlgoritm
{
    public partial class DataStructure : UserControl
    {
        public DataStructure()
        {
            InitializeComponent();
        }
        private void InfoAlgoritm_Clock(object? sender, RoutedEventArgs e)
        {
            var algoritmPage = new Algoritm();
            NavigatePage(algoritmPage);
        }

        private void BackToHome_Click(object? sender, RoutedEventArgs e)
        {
            var mainWindow = this.GetVisualRoot() as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new HomePage());
                return;
            }
            
            var mainContainer = FindParent<ContentControl>(this);
            if (mainContainer != null)
            {
                mainContainer.Content = new HomePage();
            }
        }

        private void NavigatePage(UserControl page)
        {
            var mainWindow = this.GetVisualRoot() as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(page);
                return;
            }
            
            var mainContainer = FindParent<ContentControl>(this);
            if (mainContainer != null)
            {
                mainContainer.Content = page;
            }
        }
        
        private T? FindParent<T>(Control control) where T : class
        {
            // Check visual hierarchy
            var current = control;
            while (current != null)
            {
                if (current is T result)
                    return result;
                
                if (current.Parent is T parentResult)
                    return parentResult;
                    
                current = current.Parent as Control;
            }
            
            return null;
        }
    }
}