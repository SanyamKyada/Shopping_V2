let validateInpOnKeyUp_isOn = false;

$('#btnSubmitReg').live("click", function () {

    const isValid = validateInputs();
    if (isValid) {
        let url = window.location.pathname;
        if (url != '/Account/Register' && url != '/Account/Login') {
            const url = $('#isRegisterEvent').val() == 'true' ? '/Account/Register' : '/Account/Login';
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: $("#signupForm").serialize(),
            success: function (response, textStatus, xhr) {
                if (response.from && response.from == 'register') {
                    window.location.pathname = '/Home/Index';
                }
                else if (response.token) { // check if token is sent.
                    localStorage.setItem('token', response.token);
                    if (response.returnUrl) {
                        window.location.href = response.returnUrl;
                    } else {
                        window.location.pathname = '/Home/Index';
                    }
                }
                else {
                    $('.form-messages').text("Login unsuccessfull");
                }
            },
            error: function (xhr, Message) {
                const response = JSON.parse(xhr.responseText)
                $('.form-messages').text(response.message);
            }
        });
    }
})

function validateInputs() {
    let isAllValid = true;
    const requiredInputs = $('.signup-box #signupForm input:not([type="hidden"])');;
    requiredInputs.each(function () {
        isAllValid = performValidations($(this));
    });
    validateInpOnKeyUp_isOn = !isAllValid;
    return isAllValid;
}

$('.signup-box #signupForm input:not([type="hidden"])').live('keyup', function () {
    if (validateInpOnKeyUp_isOn) {
        // If validating on keyup, perform validations only on the current input
        performValidations($(this));
    }
});

function performValidations(input) {
    const val = input.val();
    const name = input.attr('name');

    if (val.length === 0) {
        input.addClass('error');
        input.parent().prev('.font-size-14').text('This filed is required');
        return false;
    }

    switch (name) {
        case 'Name':
            input.parent().prev('.font-size-14').text('Full Name');
            input.removeClass('error');
            return true;

        case 'Email':
            const isValidEmail = validateEmail(val);
            input.parent().prev('.font-size-14').text(isValidEmail ? 'Email Address' : 'Invalid email');
            input.toggleClass('error', !isValidEmail);
            return isValidEmail;

        case 'Password':
            const isPasswordValid = validatePassword(val);
            input.parent().prev('.font-size-14').text(isPasswordValid ? 'Password' : 'Password must have at least one uppercase letter, one lowercase letter, one number, one special character, and be at least 8 characters long.');
            input.toggleClass('error', !isPasswordValid);
            return isPasswordValid;

        //case 'PasswordConfirm':
        //        const password = $('.form input[name="Password"]').val();
        //        const doPasswordsMatch = (val === password);
        //        input.prev('label').text(doPasswordsMatch ? 'Confirm Password' : 'Password & Confirm Password do not match.');
        //        input.closest('.input-box').toggleClass('error', !doPasswordsMatch);
        //    return doPasswordsMatch;

        default:
            break;
    }
}

function validateEmail(email) {
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

function validatePassword(password) {
    var passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])[A-Za-z\d!@#$%^&*()]{8,}$/;
    return passwordPattern.test(password);
}

$('#showHidePassword').live('click' , function () {
    debugger;
    if ($(this).children().hasClass('fa fa-eye-slash')) {
        $(this).children().removeClass('fa fa-eye-slash');
        $(this).children().addClass('fa fa-eye');

        var $passwordInput = $(this).prev("input[type='password']");
        var $newPasswordInput = $('<input>').attr({
            'type': 'text',
            'id': $passwordInput.attr('id'),
            'name': $passwordInput.attr('name'),
            'value': $passwordInput.val(),
            'class': $passwordInput.attr('class'),
            'placeholder': $passwordInput.attr('placeholder')
        });
        $passwordInput.replaceWith($newPasswordInput);

    } else {
        $(this).children().removeClass('fa fa-eye');
        $(this).children().addClass('fa fa-eye-slash');

        var $passwordInput = $(this).prev("input[type='text']");
        var $newPasswordInput = $('<input>').attr({
            'type': 'password',
            'id': $passwordInput.attr('id'),
            'name': $passwordInput.attr('name'),
            'value': $passwordInput.val(),
            'class': $passwordInput.attr('class'),
            'placeholder': $passwordInput.attr('placeholder')
        });
        $passwordInput.replaceWith($newPasswordInput);
    }
});