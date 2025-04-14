using ReactiveUI;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class HomePageViewModel : ReactiveObject
    {
        // Expose the LanguageManager to the view for binding
        public LanguageManager LanguageManager => LanguageManager.Instance;
        
        public HomePageViewModel()
        {
            // Підписуємося на подію зміни мови, щоб оновити інтерфейс
            LanguageManager.Instance.PropertyChanged += (sender, args) => 
            {
                if (args.PropertyName == nameof(LanguageManager.CurrentLanguage))
                {
                    this.RaisePropertyChanged(nameof(LanguageManager));
                }
            };
        }
    }
}