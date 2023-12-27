$('#rgstr-submit').live("click", function () {
    const stepNum = $('.form').data('step');
    if (stepNum === 1) {
        $('.form').css('transform', 'translate3d(-677px, 0px, 0px)');
        $(this).text('Submit');
    }
    else {
        const formData = $('.form').serialize();
        $.ajax({
            url: '/Account/Register',
            type: 'POST',
            data: formData,
            success: function (response, textStatus, xhr) {
                debugger;
            },
            error: function (xhr, Message) {
                console.log('Server error in registration');
            }
        });
    }
})