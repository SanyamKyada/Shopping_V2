$(document).ready(function () {
    var productImages = $('.mainimg,.psimages');
    $.each(productImages, function (i, elem) {
        setImageHeigthWidth(elem);
    });

    $('[id^="sku-image-"]').click(function () {
        var id = $(this).attr('id').split('-')[2];
        $('#review-form-' + id).show();
    });

    for (let i = 0; i < 100; i++) {
        $("#sku-image-" + i).click(function (e) {
            e.preventDefault();
            var currentThumb = $(this).attr("src");

            $(this).attr("src", $(this).closest(".productimages").find(".mainimg img").attr("src"));
            $(this).closest(".productimages").find(".mainimg img").attr("src", currentThumb);

        });
    }

    var id = window.location.href.split('/')[7];
    if (id != undefined || id != null) {
        $('.inactiveposts-' + id).css("background", "#f7c34c");
    }
    else {
        $(".inactiveposts-0").css("background", "#f7c34c");
    }
});

$('#addToCartBtn').click(function (e) {
    e && e.preventDefault();

    var uName = $("#userName").val();
    if (!uName) {
        window.location.pathname = '/Account/Login';
        return;
    }

    var CartData = $(this).closest('form').serialize();
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        //dataType: "json",
        data: CartData,
        success: function (response) {
            $.ajax({
                type: "GET",
                url: "/Cart/getCartPopup",
                data: { UserId: response.userId },
                success: function (result2) {

                    console.log('reached at inner ajax');
                    $('#cartModal').html('');

                    $("#cartModal").html(result2);
                    bindCloseEvent();

                    $("#cartModal").modal('show');
                },
                error: function (result2) {
                    console.log("Error");
                }
            });
        },
        error: function (xhr, status, error) {
            console.log('Error:', error);
        }
    });
});

const bindCloseEvent = () => {
    $('#closeCartModal').click(function () {
        $("#cartModal").modal('hide');
    });
}

function updateSKU(id, element) {

    $.ajax({
        url: '/Product/GetSKU',
        type: 'POST',
        data: { id: id },
        success: function (result) {
            $("#sku-container").html(result);
            window.scrollTo(100, 300)
            $(this).removeClass('navigation');
            $(this).addClass('activeSKU');
        },
        error: function (error) {
            console.log(error);
        }
    });
}

// Colour Dropdown
function selectColors(e) {
    var title = "";
    var skuName = "";
    for (let i = 0; i < e.currentTarget.options.length; i++) {

        if (e.currentTarget.options[i].dataset.title != undefined)
            title = e.currentTarget.options[i].dataset.title.replace(' ', '-');
        if (e.currentTarget.options[i].dataset.name != undefined)
            skuName = e.currentTarget.options[i].dataset.name.replace(' ', '-');
    }
    if (title != "" && skuName != " ") {
        var ProdId = $("#test1").val();
        var selector = window.location.href.split('/')[7];
        window.location.href = '/ProductDetails/' + title + '/' + skuName + '/' + ProdId + '/' + selector;
        window.scrollTo(100, 300);
    }
}

//$(function () {
//    $('[id^="inactivepostsId-"]').click(function () {
//        var id = $(this).attr('id').split('-')[1];
//        $('.inactiveposts-' + id).css("background", "#f7c34c");
//    });
//});

$(function () {
    $('[id^="review-open-"]').click(function () {
        var id = $(this).attr('id').split('-')[2];
        $('#review-form-' + id).show();
    });

    $('[id^="review-close-"]').click(function () {
        var id = $(this).attr('id').split('-')[2];
        $('#review-form-' + id).hide();
    });
});

$('[id^="reviewsubmit-"]').click(function (e) {

    if ($(this).closest('form').find('.star-rating-control').length > 0) {
        if ($('input[name="star1"]:checked').length === 0) {
            $('#reviewErrorMessage').text("You must give a rating");
            return false;
        }
    }

    var uName = $("#userName").val();

    if (!uName) {
        window.location.pathname = '/Account/Login';
        return;
    }

    e.preventDefault();
    var id = $(this).attr('id').split('-')[1];
    var selector = window.location.href.split('/')[7];
    $("#selectorId-" + id).val(selector);
    $("#formReview-" + id).trigger('submit');

});

const sortCartItems = (e) => {
    var selectedValue = e.target.value;
    var $cardContainer = $('.prd-card-container');
    var $cards = $cardContainer.children('.card');

    $cards.sort(function (a, b) {
        var valueA = $(a).data(selectedValue);
        var valueB = $(b).data(selectedValue);

        if (selectedValue === 'price') {
            valueA = parseFloat(valueA);
            valueB = parseFloat(valueB);
        } else if (selectedValue === 'name') {
            valueA = valueA.toLowerCase();
            valueB = valueB.toLowerCase();
        }

        if (valueA < valueB) {
            return -1;
        }
        if (valueA > valueB) {
            return 1;
        }
        return 0;
    });

    $cardContainer.empty().append($cards);
}