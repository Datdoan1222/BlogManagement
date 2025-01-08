// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#tblData').DataTable();
});

//Click event handler for nav-items
$('.nav-item').on('click', function () {

    //Remove any previous active classes
    $('.nav-item').removeClass('active');

    //Add active class to the clicked item
    $(this).addClass('active');
});


	