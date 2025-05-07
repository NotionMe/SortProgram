using Avalonia.Media;
using ReactiveUI;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class ArrayElementViewModel : ReactiveObject
    {
        private int _value;
        public int Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        private IBrush _background = new SolidColorBrush(Colors.Blue);
        public IBrush Background
        {
            get => _background;
            set => this.RaiseAndSetIfChanged(ref _background, value);
        }

        private IBrush _foreground = new SolidColorBrush(Colors.White);
        public IBrush Foreground
        {
            get => _foreground;
            set => this.RaiseAndSetIfChanged(ref _foreground, value);
        }

        public ArrayElementViewModel(int value)
        {
            Value = value;
        }
    }
}