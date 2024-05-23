const htmlElement = document.querySelector('html');
const themeToggleButton = document.querySelector('#darkModeToggle');
const themeIcon = document.querySelector('#themeIcon');

themeToggleButton.addEventListener('click', () => {
    const isDarkMode = htmlElement.getAttribute('data-bs-theme') === 'dark';
    htmlElement.setAttribute('data-bs-theme', isDarkMode ? 'light' : 'dark');
    themeIcon.classList.toggle('fa-moon', !isDarkMode);
    themeIcon.classList.toggle('fa-sun', isDarkMode);
});


