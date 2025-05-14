
using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Practika2_OPAM_Ubohyi_Stanislav.ErrorMessage;

public partial class ErrorMessage : Window
{
    public ErrorMessage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = this.VisualRoot as Window;
        window?.Close();
    }

    public static void ShowError()
    {
        var errorMessage = new Practika2_OPAM_Ubohyi_Stanislav.ErrorMessage.ErrorMessage();
        errorMessage.Show();
    }
}
