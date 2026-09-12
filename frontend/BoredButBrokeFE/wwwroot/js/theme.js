window.applyTheme = function(theme) {
    const root = document.documentElement;

    if (theme === 'dark') {
        root.setAttribute('data-theme', 'dark');
    } else {
        root.removeAttribute('data-theme');
    }

    localStorage.setItem('theme', theme);
};

window.initializeTheme = function() {
    const savedTheme = localStorage.getItem('theme') || 'light';
    window.applyTheme(savedTheme);
};

document.addEventListener('DOMContentLoaded', function() {
    window.initializeTheme();
});
