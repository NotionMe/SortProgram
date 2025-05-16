using Practika2_OPAM_Ubohyi_Stanislav.Services;
using ReactiveUI;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    // Інтерфейс для стратегії пошуку
    public interface ISearchingStrategy
    {
        void Initialize(int[] array, int valueToFind);
        bool PerformStep(int[] array, ref int comparisons);
        (int, int, int) GetHighlightIndices();
        int GetFoundIndex();
        int GetValueToFind();
    }

    public interface ISortingStrategy
    {
        void Initialize(int[] array);
        bool PerformStep(int[] array, ref int comparisons, ref int swaps);
        (int, int, int) GetHighlightIndices();
    }

    public class ViewModelBase : ReactiveObject
    {
        private readonly IAuthService _authService;
        private bool _isAdminVisible;

        public ViewModelBase()
        {
            _authService = App.GetService<IAuthService>();
            _isAdminVisible = _authService.IsAdmin();
        }
        public LanguageManager LanguageManager => LanguageManager.Instance;
        public bool IsAdminVisible => _isAdminVisible;
    }
}
