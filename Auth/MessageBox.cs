using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

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
        
        /// <summary>
        /// Shows a message box with the specified title and message.
        /// </summary>
        /// <param name="title">The title of the message box.</param>
        /// <param name="message">The message to display.</param>
        public static void Show(string message, string title = "Message")
        {
            var messageBox = new MessageBox
            {
                MessageTitle = title,
                Message = message
            };
            
            messageBox.Show();
        }
        
        /// <summary>
        /// Shows a message box asynchronously with the specified title and message.
        /// </summary>
        /// <param name="title">The title of the message box.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="parent">Optional parent window. If null, tries to use the main window.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task ShowAsync(string message, string title = "Message", Window? parent = null)
        {
            var messageBox = new MessageBox
            {
                MessageTitle = title,
                Message = message
            };
            
            var owner = parent ?? (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            if (owner != null)
            {
                return messageBox.ShowDialog(owner);
            }
            else
            {
                messageBox.Show();
                return Task.CompletedTask;
            }
        }
    }
}