/*-------------------------------------------------------------------------------------------------------------------------------|
|                                                         AddNewProduct.cshtml                                                   |
|<------------------------------------------------------------------------------------------------------------------------------*/

var skuDivIndex = 0;
var imagesDivIndex = 0;
var $btnNewVarient = $('#btn-new-varient');
var $btnCopyVarient = $('#btn-copy-varient');
var $btnDeleteVarient = $('#btn-delete-varient');
var $btnNewImages = $('#btn-new-images');
var $btnDeleteImages = $('#btn-delete-images');

$btnNewVarient.click(function () {
    AddSku();
});

$("#btnAdd5Sku").click(function () {
    for (var i = 0; i < 5; i++) {
        AddSku();
    }
});


function AddSku() {
    AddDivTag("", "", "", "", "", "", "", "", "", "");
}

function AddDivTag(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10){
    var divhtml = "<div class='varients-outer'><input type=checkbox id='check_" + skuDivIndex + "'/><div class='varients-inner'>";

    divhtml += "<input type='text' class='full req' name='Variants[" + skuDivIndex + "].VariantName' id='vname' placeholder='Variant name' value='" + value1 + "'/>"
    divhtml += "<input type='text' class='full' name='Variants[" + skuDivIndex + "].SKU' id='blastname' placeholder='SKU'  value='" + value2 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].OriginalPrice' id='bzip' placeholder='Original Price'  value='" + value3 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].SalePrice' id='bzip' placeholder='Sale Price'  value='" + value4 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].QuantityInStock' id='bzip' placeholder='Quantity in Stock'  value='" + value5 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].MinimumInventoryAlert' id='bzip' placeholder='Minimum Inventory Alert'  value='" + value6 + "'/>"
    divhtml += "<input type='text' class='half req' name='Variants[" + skuDivIndex + "].Color' id='bzip' placeholder='Color'  value='" + value7     + "'/>"
    divhtml += getStylesSelect(null, skuDivIndex);
    //divhtml += "<input type='file' class='half' name='Variants[" + skuDivIndex + "].ThumbImage' id='thumbimage_" + skuDivIndex + "' placeholder='Thumb Image'/>"
    divhtml += "<textarea type='text' class='full' name='Variants[" + skuDivIndex + "].Description' id='bzip' placeholder='Description' rows='4'  value='" + value10 + "' style='height:auto;'></textarea>"
    divhtml += "<div class='clear'></div>";
    divhtml += "</div ></div >";

    $("#skuDiv").append(divhtml);

    bindClickEvents();
    skuDivIndex++;
}

function getStylesSelect(selected, index) {
    var selectHtml = "<select class='half req ' id='' name='Variants[" + index + "].Style'>";

    selectHtml += "<option value='' selected disabled>Style</option>";
    selectHtml += "<option value='test-1'>1</option>";
    selectHtml += "<option value='test-2'>2</option>";
    selectHtml += "<option value='test-3'>3</option>";

    selectHtml += "</select>";
    return selectHtml;
}

$btnNewImages.click(function () {
    AddImages();
});

function AddImages() {
    AddImagesDivTag("");
}

function AddImagesDivTag(value1) {
    var divhtml = "<div class='images-outer'><input type=checkbox id='check_" + imagesDivIndex + "'/><div class='images-inner'><div class=' d-flex'>";

    divhtml += getColorsSelect(null, imagesDivIndex);
    divhtml += "<lable>Thumb Image</lable>"
    divhtml += "<input type='file' class='half req' name='Images[" + imagesDivIndex + "].ThumbImage' id='thumbimage_" + imagesDivIndex + "' placeholder='Thumb Image'/>"
    divhtml += "<lable>Side Images</lable>"
    divhtml += "<input type='file' class='half' name='Images[" + imagesDivIndex + "].SideImage' id='sideimage_" + imagesDivIndex + "' placeholder='Side Images' multiple/></div >"
    divhtml += "<div class='img-upload-show'><div id='display_thumbimg_" + imagesDivIndex + "'></div><div class='sideimg-container' id='display_sideimg_" + imagesDivIndex + "'></div></div>"
    divhtml += "<div class='clear'></div>";
    divhtml += "</div ></div >";

    $("#imagesDiv").append(divhtml);

    bindClickEvents();
    imagesDivIndex++;
}

function getColorsSelect(selected, index) {
    var selectHtml = "<select class='half req ' id='' name='Images[" + index + "].Color'>";

    selectHtml += "<option value='' selected disabled>Color</option>";
    selectHtml += "<option value='color-1'>Red</option>";
    selectHtml += "<option value='color-2'>Blue</option>";
    selectHtml += "<option value='color-3'>Green</option>";

    selectHtml += "</select>";
    return selectHtml;
}

function bindClickEvents() {
    $('.varient-detail [id^="check_"]').click(setBtnVarientCopyDeleteCursor);
    $('.varient-images [id^="check_"]').click(setBtnDeleteImagesCursor);

    $btnDeleteVarient.click(function () {
        var $checkedInputs = $('.varient-detail [id^="check_"]:checked');

        $.each($checkedInputs, function (i, elem) {
            $(this).closest('div.varients-outer').remove();
        });

        setBtnVarientCopyDeleteCursor();
    });

    $btnDeleteImages.click(function () {
        var $checkedInputs = $('.varient-images [id^="check_"]:checked');

        $.each($checkedInputs, function (i, elem) {
            $(this).closest('div.images-outer').remove();
        });

        setBtnDeleteImagesCursor();
    });

    $btnCopyVarient.click(function () {
        var $checkedInput = $('.varient-detail [id^="check_"]');
        //CloneDivTag()
    });
}

$('[id^="thumbimage_"]').live('change', function () {
        var $thumbImage = $(this);
        var uploadedFile = $thumbImage.prop('files');
        var currentElemIndex = $(this).attr('id').split('_')[1];

        if (uploadedFile.length > 0) {
            var fileUrl = URL.createObjectURL(uploadedFile[0]);

            var img = new Image();
            img.src = fileUrl;

            img.onload = function () {
                debugger;
                var maxSize = 200; 
                var width = img.width;
                var height = img.height;

                var scale = Math.min(maxSize / width, maxSize / height);

                var scaledWidth = width * scale;
                var scaledHeight = height * scale;

                var $container = $('<div></div>')
                    .css({
                        width: maxSize + 'px',
                        height: maxSize + 'px',
                        overflow: 'hidden',
                        position: 'relative',
                    });

                var $centeredImage = $('<img>').attr('src', fileUrl)
                    .css({
                        width: scaledWidth + 'px',
                        height: scaledHeight + 'px',
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                    });

                $container.append($centeredImage);

                $('#display_thumbimg_' + currentElemIndex).empty().append($container);
            };
        }
});

$('[id^="sideimage_"]').live('change', function () {
    var $sideImagesInp = $(this);
    var uploadedFile = $sideImagesInp.prop('files');
    var currentElemIndex = $(this).attr('id').split('_')[1];

    if (uploadedFile.length > 0) {

        $.each(uploadedFile, function (i, elem) {
            var fileUrl = URL.createObjectURL(elem);

            var img = new Image();
            img.src = fileUrl;

            img.onload = function () {
                debugger;
                var maxSize = 100; 
                var width = img.width;
                var height = img.height;

                var scale = Math.min(maxSize / width, maxSize / height);

                var scaledWidth = width * scale;
                var scaledHeight = height * scale;

                var $centeredImage = $('<img>').attr('src', fileUrl)
                    .css({
                        width: scaledWidth + 'px',
                        height: scaledHeight + 'px',
                    });

                $('#display_sideimg_' + currentElemIndex).append($centeredImage);
            };
        });
    }
});

//if the div is not the perfect square

//img.onload = function () {
//    var width = img.width;
//    var height = img.height;

//    // Calculate the scaling factors for width and height
//    var scaleWidth = maxWidth / width;
//    var scaleHeight = maxHeight / height;

//    // Choose the smaller scaling factor to fit both width and height within the specified dimensions
//    var scale = Math.min(scaleWidth, scaleHeight);

//    // Scale the image
//    var scaledWidth = width * scale;
//    var scaledHeight = height * scale;

//    // Create a container div with specified width and height
//    var $container = $('<div></div>')
//        .css({
//            width: maxWidth + 'px', // Adjust to the desired width
//            height: maxHeight + 'px', // Adjust to the desired height
//            overflow: 'hidden',
//            position: 'relative',
//        });

//    // Create an image element and center it both horizontally and vertically
//    var $centeredImage = $('<img>').attr('src', fileUrl)
//        .css({
//            width: scaledWidth + 'px',
//            height: scaledHeight + 'px',
//            position: 'absolute',
//            top: '50%',
//            left: '50%',
//            transform: 'translate(-50%, -50%)',
//        });

//    // Append the centered image to the container
//    $container.append($centeredImage);

//    // Replace the contents of the image container with the scaled and centered image
//    $imageContainer.empty().append($container);
//};

function setBtnVarientCopyDeleteCursor() {
    var $checkedInputs = $('[.varient-detail id^="check_"]:checked');

    if ($checkedInputs.length == 0) {
        $btnDeleteVarient.css('cursor', 'not-allowed');
        $btnCopyVarient.css('cursor', 'not-allowed');
    } else if ($checkedInputs.length == 1) {
        $btnDeleteVarient.css('cursor', 'pointer');
        $btnCopyVarient.css('cursor', 'pointer');
    }
    else {
        $btnDeleteVarient.css('cursor', 'pointer');
        $btnCopyVarient.css('cursor', 'not-allowed');
    }
}

function setBtnDeleteImagesCursor() {
    var $checkedInputs = $('.varient-images [id^="check_"]:checked');

    if ($checkedInputs.length == 0) {
        $btnDeleteImages.css('cursor', 'not-allowed');
    }
    else {
        $btnDeleteImages.css('cursor', 'pointer');
    }
}

$("#btnSkuFormSubmit").click(function () {
    $(".form-sku").submit();
});


