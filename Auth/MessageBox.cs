using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth
{
    public partial class MessageBox : Window
    {
        private TextBlock? _titleTextBlock;
        private TextBlock? _messageTextBlock;

        public string MessageTitle { get; set; } = "Message";
        public string Message { get; set; } = string.Empty;
        
        public MessageBox()
        {
            InitializeComponent();
            
            _titleTextBlock = this.FindControl<TextBlock>("TitleTextBlock");
            _messageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");
            
            this.Title = MessageTitle;
            
            // Застосування значень напряму до елементів інтерфейсу
            Loaded += (s, e) => 
            {
                if (_titleTextBlock != null)
                    _titleTextBlock.Text = MessageTitle;
                    
                if (_messageTextBlock != null)
                    _messageTextBlock.Text = Message;
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OkButton_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}