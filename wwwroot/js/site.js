// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Site-wide JavaScript for Course Management Portal

// Auto-dismiss alerts after 5 seconds
$(document).ready(function () {
    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 5000);

    // Confirm delete actions
    $('.delete-btn').on('click', function (e) {
        if (!confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
            e.preventDefault();
        }
    });

    // Form validation enhancement
    $('form').on('submit', function () {
        $(this).find('button[type="submit"]').prop('disabled', true);
    });
});