using System;
using Avalonia.Controls;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms.Searching;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers
{
    public static class NavigationHandlers
    {
        public static void NavigateToPage(SortProgram? mainWindow, UserControl page)
        {
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(page);
            }
        }
    }
}