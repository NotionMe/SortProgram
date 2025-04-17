using Practika2_OPAM_Ubohyi_Stanislav.Services;
using ReactiveUI;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    // Інтерфейс для стратегії сортування
    public interface ISortingStrategy
    {
        // Ініціалізація сортування
        void Initialize(int[] array);
        
        // Виконання одного кроку сортування
        bool PerformStep(int[] array, ref int comparisons, ref int swaps);
        
        // Отримання індексів елементів для візуалізації кольорів
        (int, int, int) GetHighlightIndices();
    }

    public class ViewModelBase : ReactiveObject
    {
        private readonly AuthService _authService;
        private bool _isAdminVisible;

        public ViewModelBase()
        {
            _authService = AuthService.Instance;
            _isAdminVisible = _authService.IsAdmin();
        }

        // Add a property to access LanguageManager from the view
        public LanguageManager LanguageManager => LanguageManager.Instance;
        
        // Property to control visibility of admin button
        public bool IsAdminVisible => _isAdminVisible;
    }
}
