/*-------------------------------------------------------------------------------------------------------------------------------|
|                                                         AddNewProduct.cshtml                                                   |
|<------------------------------------------------------------------------------------------------------------------------------*/

var skuDivIndex = 0;

$("#btnAddSku").click(function () {
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
    var divhtml = "<div class='dashed-outer'><div class='dashed-inner'>"; 

    divhtml = divhtml + "<input type='text' class='full req' name='Variants[" + skuDivIndex + "].VariantName' id='vname' placeholder='Variant name' value='" + value1 + "'/>"
    divhtml = divhtml + "<input type='text' class='full' name='Variants[" + skuDivIndex + "].SKU' id='blastname' placeholder='SKU'  value='" + value2 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].OriginalPrice' id='bzip' placeholder='Original Price'  value='" + value3 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].SalePrice' id='bzip' placeholder='Sale Price'  value='" + value4 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].QuantityInStock' id='bzip' placeholder='Quantity in Stock'  value='" + value5 + "'/>"
    divhtml = divhtml + "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].MinimumInventoryAlert' id='bzip' placeholder='Minimum Inventory Alert'  value='" + value6 + "'/>"
    divhtml = divhtml + "<input type='file' class='half req' name='Variants[" + skuDivIndex + "].ThumbImage' id='bzip' placeholder='Thumb Image'/>"
    divhtml = divhtml + "<input type='text' class='half req' name='Variants[" + skuDivIndex + "].Color' id='bzip' placeholder='Color'  value='" + value8 + "'/>"
    divhtml = divhtml + getStylesSelect(null, skuDivIndex);
    divhtml = divhtml + "<textarea type='text' class='full' name='Variants[" + skuDivIndex + "].Description' id='bzip' placeholder='Description' rows='4'  value='" + value10 + "' style='height:auto;'></textarea>"
    divhtml = divhtml + "<div class='clear'></div>"
    divhtml = divhtml + "</div ></div >";

    $("#skuDiv").append(divhtml);
    skuDivIndex++;
}

function getStylesSelect(selected, index) {
    var selectHtml = "<select class='half' id='' name='Variants[" + index + "].Style'>";

    selectHtml = selectHtml + "<option value='' selected disabled>Style</option>";
    selectHtml = selectHtml + "<option value='test-1'>1</option>";
    selectHtml = selectHtml + "<option value='test-2'>2</option>";
    selectHtml = selectHtml + "<option value='test-3'>3</option>";

    selectHtml = selectHtml + "</select>";
    return selectHtml;
}