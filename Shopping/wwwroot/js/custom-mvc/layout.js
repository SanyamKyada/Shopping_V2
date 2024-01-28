$(document).ready(function () {

    $('#acceptCookieButton').live('click', function () {
        document.cookie = 'consent=accepted; expires=Fri, 31 Dec 9999 23:59:59 GMT; path=/';

        $('#cookieConsent').hide();
    });

    if (document.cookie.split(';').some((item) => item.trim().startsWith('consent='))) {
        $('#cookieConsent').hide();
    }
});

// coockie consent
$('#acpt-btn').click(function () {
    $('#cookieConsent').remove();
});