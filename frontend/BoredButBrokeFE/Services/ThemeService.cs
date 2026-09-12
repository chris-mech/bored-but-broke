using Microsoft.JSInterop;

namespace BoredButBrokeFE.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private string _currentTheme = "light";

        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeThemeAsync()
        {
            try
            {
                _currentTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme") ?? "light";
                if (string.IsNullOrEmpty(_currentTheme))
                {
                    _currentTheme = "light";
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", "light");
                }
                await ApplyThemeAsync(_currentTheme);
            }
            catch
            {
                _currentTheme = "light";
            }
        }

        public async Task ToggleThemeAsync()
        {
            _currentTheme = _currentTheme == "light" ? "dark" : "light";
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _currentTheme);
            await ApplyThemeAsync(_currentTheme);
            OnThemeChanged?.Invoke();
        }

        public string GetCurrentTheme() => _currentTheme;

        private async Task ApplyThemeAsync(string theme)
        {
            await _jsRuntime.InvokeVoidAsync("applyTheme", theme);
        }
    }
}
