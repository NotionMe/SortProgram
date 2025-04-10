using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }
        
        // Метод-заглушка для обробки натискання на кнопку "Bubble Sort Info"
        private void BubbleSortButton_Click(object sender, RoutedEventArgs e)
        {
            // Логіка для кнопки "Bubble Sort Info" тут
            System.Diagnostics.Debug.WriteLine("Натиснуто на кнопку Bubble Sort Info");
        }
    }
}
