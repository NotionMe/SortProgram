
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Practika2_OPAM_Ubohyi_Stanislav.AccountDelete
{
    public partial class MessageDelete : Window
    {
        private TextBlock? _titleTextBlock;
        private TextBlock? _messageTextBlock;

        // Result will be true if user clicked "Yes", false if clicked "No" or closed the dialog
        public bool Result { get; private set; } = false;

        public string MessageTitle { get; set; } = "Видалення профілю";
        public string Message { get; set; } = "Ви дійсно хочете видалити профіль?";

        public MessageDelete()
        {
            InitializeComponent();

            _titleTextBlock = this.FindControl<TextBlock>("TitleTextBlock");
            _messageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");

            this.Title = MessageTitle;

            // Apply values directly to UI elements
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

        private void YesButton_Click(object? sender, RoutedEventArgs e)
        {
            Result = true;
            this.Close();
        }

        private void NoButton_Click(object? sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();
        }

        /// <summary>
        /// Shows a confirmation dialog and returns true if user selects "Yes", false otherwise.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <returns>True if the user clicks "Yes", false otherwise.</returns>
        public static async Task<bool> ShowAsync(string message = "Ви дійсно хочете видалити профіль?", string title = "Видалення профілю")
        {
            var messageBox = new MessageDelete
            {
                MessageTitle = title,
                Message = message
            };

            // Find a valid owner window that is visible
            Window? validOwner = null;

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (Window window in desktop.Windows)
                {
                    if (window.IsVisible)
                    {
                        validOwner = window;
                        break;
                    }
                }
            }

            try
            {
                if (validOwner != null)
                {
                    await messageBox.ShowDialog(validOwner);
                }
                else
                {
                    // If we can't find a valid owner, show as a regular window
                    messageBox.Show();
                    // Wait for the window to close
                    while (messageBox.IsVisible)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            catch (System.InvalidOperationException)
            {
                // If showing the dialog fails, show as a regular window
                messageBox.Show();
                // Wait for the window to close
                while (messageBox.IsVisible)
                {
                    await Task.Delay(100);
                }
            }

            return messageBox.Result;
        }
    }
}