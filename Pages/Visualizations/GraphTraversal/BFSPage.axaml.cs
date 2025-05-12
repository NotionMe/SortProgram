using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.GraphTraversal;

public partial class BFSPage : UserControl
{
    public BFSPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
