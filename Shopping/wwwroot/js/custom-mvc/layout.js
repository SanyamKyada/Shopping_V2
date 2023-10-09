$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Account/GetLoginPopup",
        success: function (result) {
            $("#login-popup").html(result);
            loginAction();
            onEyeToggle();
        },
        error: function (result) {
            console.log("Error");
        }
    });
});

function loginAction() {
    $('[id^="LoginForm"]').submit(function (e) {
        e && e.preventDefault();
        if ($(this).find("#username").val() == "") {
            $('#userError').text("Username should not be empty");
            return false;
        }
        if ($(this).find("#password1").val() == "") {
            $('#userError').text("Password should not be empty");
            return false;
        }

        var formData = $(this).serialize();

        $.ajax({
            url: '/Account/Login',
            type: 'POST',
            data: formData,
            success: function (response, textStatus, xhr) {
                debugger;
                $('#userError').hide();
                if (response.token) {
                    localStorage.setItem('token', response.token);
                    if (response.returnUrl) {
                        window.location.href = response.returnUrl;
                    } else {
                        location.reload();
                    }
                } else {
                    $('#loginErrorMessage').text("Login unsuccessfull");
                }
            },
            error: function (xhr, Message) {
                $('#loginErrorMessage').text("Login unsuccessfull");
            }
        });
    });

}

function onEyeToggle() {
    $('#eye-toggle').click(function () {
        debugger;
        if ($(this).hasClass('fa fa-eye-slash')) {
            $(this).removeClass('fa fa-eye-slash');
            $(this).addClass('fa fa-eye');

            var $passwordInput = $(this).parent().prevAll("input[type='password'][id='password1']");
            var $newPasswordInput = $('<input>').attr({
                'type': 'text',
                'id': $passwordInput.attr('id'),
                'name': $passwordInput.attr('name'),
                'value': $passwordInput.val(),
                'class': $passwordInput.attr('class')
            });
            $passwordInput.replaceWith($newPasswordInput);

        } else {
            $(this).removeClass('fa fa-eye');
            $(this).addClass('fa fa-eye-slash');

            var $passwordInput = $(this).parent().prevAll("input[type='text'][id='password1']");
            var $newPasswordInput = $('<input>').attr({
                'type': 'password',
                'id': $passwordInput.attr('id'),
                'name': $passwordInput.attr('name'),
                'value': $passwordInput.val(),
                'class': $passwordInput.attr('class')
            });
            $passwordInput.replaceWith($newPasswordInput);
        }
    });
}

$("#login-btn").click(function () {
    $("#login-popup").modal('show');
    $("#username").val("");
    $("#password1").val("");
    $("#loginErrorMessage").text("");
});

// coockie consent
$('#acpt-btn').click(function () {
    $('#cookieConsent').remove();
});

//if (slideTo == 3) {
//    $(".stage4 .infiscroll > div.infiscroll-inner, .stage4 .infiscroll > div.infiscroll-inner img").addClass("animate");
//}
//else {
//    $(".stage4 .infiscroll > div.infiscroll-inner, .stage4 .infiscroll > div.infiscroll-inner img").removeClass("animate");
//}