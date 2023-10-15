/*-------------------------------------------------------------------------------------------------------------------------------|
|                                                         AddNewProduct.cshtml                                                   |
|<------------------------------------------------------------------------------------------------------------------------------*/

var skuDivIndex = 0;
var $btnNewVarient = $('#btn-new-varient');
var $btnCopyVarient = $('#btn-copy-varient');
var $btnDeleteVarient = $('#btn-delete-varient');

$btnNewVarient.click(function () {
    AddSku();
});

$("#btnAdd5Sku").click(function () {
    for (var i = 0; i < 5; i++) {
        AddSku();
    }
});

$("#btnSkuFormSubmit").click(function () {
    $(".form-sku").submit();
});

function AddSku() {
    AddDivTag("", "", "", "", "", "", "", "", "", "");
}

function AddDivTag(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10){
    var divhtml = "<div class='dashed-outer'><input type=checkbox id='check_" + skuDivIndex + "'/><div class='dashed-inner'>"; 

    divhtml = divhtml + "<input type='text' class='full req' name='Variants[" + skuDivIndex + "].VariantName' id='vname' placeholder='Variant name' value='" + value1 + "'/>"
    divhtml = divhtml + "<input type='text' class='full' name='Variants[" + skuDivIndex + "].SKU' id='blastname' placeholder='SKU'  value='" + value2 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].OriginalPrice' id='bzip' placeholder='Original Price'  value='" + value3 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].SalePrice' id='bzip' placeholder='Sale Price'  value='" + value4 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].QuantityInStock' id='bzip' placeholder='Quantity in Stock'  value='" + value5 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].MinimumInventoryAlert' id='bzip' placeholder='Minimum Inventory Alert'  value='" + value6 + "'/>"
    divhtml = divhtml + "<input type='text' class='half req' name='Variants[" + skuDivIndex + "].Color' id='bzip' placeholder='Color'  value='" + value7     + "'/>"
    divhtml = divhtml + getStylesSelect(null, skuDivIndex);
    divhtml = divhtml + "<input type='file' class='half' name='Variants[" + skuDivIndex + "].ThumbImage' id='thumbimage_" + skuDivIndex + "' placeholder='Thumb Image'/>"
    divhtml = divhtml + "<textarea type='text' class='full' name='Variants[" + skuDivIndex + "].Description' id='bzip' placeholder='Description' rows='4'  value='" + value10 + "' style='height:auto;'></textarea>"
    //divhtml = divhtml + "<div class='img-upload-show' id='display_thumbimg_" + skuDivIndex + "'><img src='' /></div>"
    divhtml = divhtml + "<div class='clear'></div>";
    divhtml = divhtml + "</div ></div >";

    $("#skuDiv").append(divhtml);

    bindClickEvents();
    skuDivIndex++;
}

function getStylesSelect(selected, index) {
    var selectHtml = "<select class='half req ' id='' name='Variants[" + index + "].Style'>";

    selectHtml = selectHtml + "<option value='' selected disabled>Style</option>";
    selectHtml = selectHtml + "<option value='test-1'>1</option>";
    selectHtml = selectHtml + "<option value='test-2'>2</option>";
    selectHtml = selectHtml + "<option value='test-3'>3</option>";

    selectHtml = selectHtml + "</select>";
    return selectHtml;
}

function bindClickEvents() {
    $('[id^="check_"]').click(function () {

        var $currentCheckbox = $(this);
        var $checkedInputs = $('[id^="check_"]:checked');

        if ($checkedInputs.length == 0) {
            $btnDeleteVarient.css('cursor', 'not-allowed');
            $btnCopyVarient.css('cursor', 'not-allowed');
        } else if ($checkedInputs.length == 1) {
            $btnDeleteVarient.css('cursor', 'pointer');
            $btnCopyVarient.css('cursor', 'pointer');
        }
        else {
            $btnDeleteVarient.css('cursor', 'pointer');
            $btnCopyVarient.css('cursor','not-allowed');
        }
    });

    $("#btn-delete-varient").click(function () {
        var $checkedInputs = $('[id^="check_"]:checked');

        $.each($checkedInputs, function (i, elem) {
            $(this).closest('div.dashed-outer').remove();
        });
    });

    $("#btn-copy-varient").click(function () {
        var $checkedInput = $('[id^="check_"]');
        //AddDivTag()
    });

    //$('[id^="thumbimage_"]').live('change', function () {
    //    debugger;
    //    var $thumbImage = $(this);
    //    var uploadedFile = $thumbImage.prop('files');
    //    var currentElemIndex = $(this).attr('id').split('_')[1];

    //    if (uploadedFile.length > 0) {
    //        var fileUrl = URL.createObjectURL(uploadedFile[0]);
    //        $('#display_thumbimg_' + currentElemIndex).find('img').prop('src', fileUrl);
    //    }
    //});
}


