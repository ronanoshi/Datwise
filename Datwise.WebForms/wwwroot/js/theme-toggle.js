// Theme Toggle Functionality
(function() {
    // Get the saved theme from localStorage or default to 'dark'
    const savedTheme = localStorage.getItem('theme') || 'dark';
    
    // Apply the saved theme on page load
    document.documentElement.setAttribute('data-theme', savedTheme);
    updateThemeSwitch(savedTheme);
    
    // Theme toggle switch change handler
    const themeCheckbox = document.getElementById('themeCheckbox');
    if (themeCheckbox) {
        themeCheckbox.addEventListener('change', toggleTheme);
    }
    
    function toggleTheme() {
        const currentTheme = document.documentElement.getAttribute('data-theme') || 'dark';
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        // Update the HTML attribute
        document.documentElement.setAttribute('data-theme', newTheme);
        
        // Save to localStorage
        localStorage.setItem('theme', newTheme);
        
        // Update switch state
        updateThemeSwitch(newTheme);
    }
    
    function updateThemeSwitch(theme) {
        const checkbox = document.getElementById('themeCheckbox');
        if (checkbox) {
            checkbox.checked = theme === 'light';
        }
    }
})();
