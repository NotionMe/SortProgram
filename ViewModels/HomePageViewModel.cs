using Practika2_OPAM_Ubohyi_Stanislav.Services;
using ReactiveUI;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public new LanguageManager LanguageManager => LanguageManager.Instance;
        
        public HomePageViewModel()
        {
            LanguageManager.Instance.LanguageChanged += (s, e) => this.RaisePropertyChanged(nameof(LanguageManager));
        }
    }
}