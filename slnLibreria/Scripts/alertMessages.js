$(document).ready(function () {
    $('#alertMessage').delay(15000).fadeOut();

    $("#warning-alert").fadeTo(10000, 500).slideUp(500, function () {
        $("#warning-alert").slideUp(500);
    });

    $("#primary-alert").fadeTo(5000, 500).slideUp(500, function () {
        $("#primary-alert").slideUp(500);
    });

    $("#danger-alert").fadeTo(8000, 500).slideUp(500, function () {
        $("#danger-alert").slideUp(500);
    });

    $("#success-alert").fadeTo(10000, 500).slideUp(500, function () {
        $("#success-alert").slideUp(500);
    });

});

