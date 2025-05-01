using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.InfoAlgoritm;

public partial class Algoritm : UserControl
{
    public Algoritm()
    {
        InitializeComponent();
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
    private void InfoDateStructureButton_Click(object? sender, RoutedEventArgs e)
    {
        var dataStructurePage = new DataStructure();
        NavigatePage(dataStructurePage);
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
