// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(function (element) {
    element.addEventListener('click', function () {
        var target = document.querySelector(this.getAttribute('href'));
        if (target.classList.contains('show')) {
            this.setAttribute('aria-expanded', 'false');
        } else {
            this.setAttribute('aria-expanded', 'true');
        }
    });
});
