using ReactiveUI;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Avalonia.Threading;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public new LanguageManager LanguageManager => LanguageManager.Instance;
        
        public HomePageViewModel()
        {
            // Ensure language change events are processed on UI thread
            LanguageManager.Instance.LanguageChanged += (s, e) => 
                Dispatcher.UIThread.Post(() => this.RaisePropertyChanged(nameof(LanguageManager)));
        }
    }
}