using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Admin
{
    public partial class RoleManagementPage : UserControl
    {
        private RoleManagementViewModel _viewModel;
        private WrapPanel? _userPanel;
        
        public RoleManagementPage()
        {
            InitializeComponent();
            _viewModel = new RoleManagementViewModel();
            DataContext = _viewModel;
            
            Loaded += RoleManagementPage_Loaded;
        }

        private void RoleManagementPage_Loaded(object? sender, RoutedEventArgs e)
        {
            _userPanel = this.FindControl<WrapPanel>("UserPanel");
            if (_userPanel != null)
            {
                CreateUserCards();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private IBrush GetThemeBrush(string resourceKey, IBrush fallback)
        {
            if (Application.Current != null)
            {
                if (Application.Current.TryFindResource(resourceKey, out var resource) && resource is IBrush brush)
                {
                    return brush;
                }
            }
            return fallback;
        }
        
        private void CreateUserCards()
        {
            if (_userPanel == null) return;
            
            _userPanel.Children.Clear();
            
            foreach (var userViewModel in _viewModel.UserViewModels)
            {
                // Create card border
                var border = new Border
                {
                    Background = new SolidColorBrush(Color.Parse("#424B54")),
                    CornerRadius = new CornerRadius(8),
                    Margin = new Thickness(10),
                    Padding = new Thickness(15),
                    Width = 320,
                    Child = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto },
                            new RowDefinition { Height = GridLength.Star }
                        }
                    }
                };

                var grid = (Grid)border.Child;
                
                // Header
                var headerPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 10
                };
                
                // User icon
                var viewbox = new Viewbox
                {
                    Width = 36,
                    Height = 36,
                    Child = new Canvas
                    {
                        Width = 24,
                        Height = 24,
                        Children =
                        {
                            new Path
                            {
                                Fill = GetThemeBrush("ThemeForegroundBrush", Brushes.White),
                                Data = Geometry.Parse("M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z")
                            }
                        }
                    }
                };
                
                headerPanel.Children.Add(viewbox);
                
                var headerText = new TextBlock
                {
                    Text = "Info",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 20,
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.White),
                    FontWeight = FontWeight.SemiBold
                };
                
                headerPanel.Children.Add(headerText);
                
                Grid.SetRow(headerPanel, 0);
                grid.Children.Add(headerPanel);
                
                // Content panel
                var contentPanel = new StackPanel
                {
                    Margin = new Thickness(0, 10, 0, 0)
                };
                
                // Name field
                contentPanel.Children.Add(new TextBlock { 
                    Text = "Name:", 
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray), 
                    FontSize = 14, 
                    Margin = new Thickness(0, 5, 0, 2) 
                });
                
                var nameTextBox = new TextBox
                {
                    Text = userViewModel.Username,
                    IsReadOnly = true,
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray),
                    Background = GetThemeBrush("ThemeCardBackgroundBrush", new SolidColorBrush(Color.Parse("#525D66"))),
                    Height = 32,
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(0, 0, 0, 8),
                    Padding = new Thickness(8, 6)
                };
                contentPanel.Children.Add(nameTextBox);
                
                // Email field
                contentPanel.Children.Add(new TextBlock { 
                    Text = "Email:", 
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray), 
                    FontSize = 14, 
                    Margin = new Thickness(0, 5, 0, 2) 
                });
                
                var emailTextBox = new TextBox
                {
                    Text = userViewModel.Email,
                    IsReadOnly = true,
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray),
                    Background = GetThemeBrush("ThemeCardBackgroundBrush", new SolidColorBrush(Color.Parse("#525D66"))),
                    Height = 32,
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(0, 0, 0, 8),
                    Padding = new Thickness(8, 6)
                };
                contentPanel.Children.Add(emailTextBox);
                
                // Password field (masked)
                contentPanel.Children.Add(new TextBlock { 
                    Text = "Password:", 
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray), 
                    FontSize = 14, 
                    Margin = new Thickness(0, 5, 0, 2) 
                });
                
                var passwordTextBox = new TextBox
                {
                    Text = "••••••••••",
                    IsReadOnly = true,
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray),
                    Background = GetThemeBrush("ThemeCardBackgroundBrush", new SolidColorBrush(Color.Parse("#525D66"))),
                    Height = 32,
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(0, 0, 0, 8),
                    Padding = new Thickness(8, 6)
                };
                contentPanel.Children.Add(passwordTextBox);
                
                // Role field
                contentPanel.Children.Add(new TextBlock { 
                    Text = "Role:", 
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray), 
                    FontSize = 14, 
                    Margin = new Thickness(0, 5, 0, 2) 
                });
                
                var roleComboBox = new ComboBox
                {
                    Width = 150,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray),
                    Background = GetThemeBrush("ThemeCardBackgroundBrush", new SolidColorBrush(Color.Parse("#525D66"))),
                    Height = 32,
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(0, 0, 0, 8),
                    Padding = new Thickness(8, 6)
                };
                
                // Add roles from the view model
                foreach (var role in _viewModel.AvailableRoles)
                {
                    roleComboBox.Items.Add(role);
                }
                
                // Set the selected role
                roleComboBox.SelectedItem = userViewModel.Role;
                
                contentPanel.Children.Add(roleComboBox);
                
                // Update button
                var updateButton = new Button
                {
                    Content = "Update Role",
                    Background = new SolidColorBrush(Color.Parse("#2E8BC0")),
                    Foreground = GetThemeBrush("ThemeForegroundBrush", Brushes.Gray),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Padding = new Thickness(12, 6),
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(0, 5, 0, 0)
                };
                
                updateButton.Click += (s, e) =>
                {
                    // Only process if a valid role is selected
                    if (roleComboBox.SelectedItem is string selectedRole)
                    {
                        _viewModel.SelectUser(userViewModel, selectedRole);
                        
                        // Execute the command if it can execute
                        if (_viewModel.UpdateRoleCommand.CanExecute(null))
                        {
                            _viewModel.UpdateRoleCommand.Execute(null);
                        }
                    }
                };
                
                contentPanel.Children.Add(updateButton);
                
                Grid.SetRow(contentPanel, 1);
                grid.Children.Add(contentPanel);
                
                _userPanel.Children.Add(border);
            }
        }
    }
}
