const htmlElement = document.querySelector('html');
const themeToggleButton = document.querySelector('#darkModeToggle');
const themeIcon = document.querySelector('#themeIcon');

themeToggleButton.addEventListener('click', () => {
    const isDarkMode = htmlElement.getAttribute('data-bs-theme') === 'dark';
    const newTheme = isDarkMode ? 'light' : 'dark';
    htmlElement.setAttribute('data-bs-theme', newTheme);
    themeIcon.classList.toggle('fa-moon', !isDarkMode);
    themeIcon.classList.toggle('fa-sun', isDarkMode);
    localStorage.setItem('theme', newTheme);
});

document.addEventListener('DOMContentLoaded', () => {
    const savedTheme = localStorage.getItem('theme') || 'light';
    htmlElement.setAttribute('data-bs-theme', savedTheme);
    themeIcon.classList.toggle('fa-moon', savedTheme === 'dark');
    themeIcon.classList.toggle('fa-sun', savedTheme !== 'dark');
});


