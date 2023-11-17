/*-------------------------------------------------------------------------------------------------------------------------------|
|                                                         AddNewProduct.cshtml                                                   |
|<------------------------------------------------------------------------------------------------------------------------------*/

var skuDivIndex = 0;
var imagesDivIndex = 0;
var $btnNewVarient = $('#btn-new-varient');
var $btnCopyVarient = $('#btn-copy-varient');
var $btnSaveVarient = $('#btn-save-varient');
var $btnDeleteVarient = $('#btn-delete-varient');
var $btnNewImages = $('#btn-new-images');
var $btnDeleteImages = $('#btn-delete-images');
const $categoryIdSelect = $('#pd_category')
var styleAttributes = [];
var specificationAttributes = [];
var productColours = [];
var colorsUserSelected = []; // will store the ids of color of products that user filled & then will show only those colors while adding Product Images 
                             // Will store its value when user saves Variants Detail

$(document).ready(function () {

    $('#tInput').AddTags({
        maxTags: 10,
        initialTags: [],
        includeTitle: false
    });

    $.ajax({
        url: '/Admin/GetDetails',
        type: 'GET',
        success: function (response) {
            if (response.attrsArray) {
                styleAttributes = $.grep(response.attrsArray, function (item) {
                    return item.isSpecificationAttribute === false;
                });
                specificationAttributes = $.grep(response.attrsArray, function (item) {
                    return item.isSpecificationAttribute === true;
                });
            }

            if (response.productColoursArray) {
                productColours = response.productColoursArray;
            }
        },
        error: function (xhr, Message) {
            console.log('error in admin.js $(document).ready')
        }
    });
});

$('.btn-openclose-card').click(function () {
    $('#skuDiv').css('overflow', '');
    const filterBody = $(this).closest('div.custom-card-header').next();
    if (filterBody && filterBody.hasClass('custom-card-body')) {
        if (filterBody.hasClass('hidden')) {
            filterBody.slideDown(200, function () {
                filterBody.removeClass("hidden");
            });
            $(this).find('i').removeClass('fa fa-plus');
            $(this).find('i').addClass('fa fa-minus');
        }
        else {
            filterBody.slideUp(200, function () {
                filterBody.addClass("hidden");
            });
            $(this).find('i').removeClass('fa fa-minus');
            $(this).find('i').addClass('fa fa-plus');
        }
    }
    $('#skuDiv').css('overflow', 'unset');
})

$('#skuDiv').live('DOMNodeInserted', function (event) {
    var $target = $(event.target);
    if ($target.hasClass('varients-outer')) {
        // Add border-bottom to all .varients-inner elements
        $('#skuDiv .varients-inner').css('border-bottom', '1px solid grey');

        // Remove the border-bottom from the last .varients-inner element
        $('#skuDiv .varients-inner:last').css('border-bottom', 'none');
    }
});

$('#imagesDiv').live('DOMNodeInserted', function (event) {
    var $target = $(event.target);
    if ($target.hasClass('images-outer')) {
        // Add border-bottom to all .varients-inner elements
        $('#imagesDiv .images-inner').css('border-bottom', '1px solid grey');

        // Remove the border-bottom from the last .varients-inner element
        $('#imagesDiv .images-inner:last').css('border-bottom', 'none');
    }
});

$btnNewVarient.click(function () {
    AddSku();
});

$("#btnAdd5Sku").click(function () {
    for (var i = 0; i < 5; i++) {
        AddSku();
    }
});


function AddSku() {
    var categorySelected = $categoryIdSelect.val();
    if (!categorySelected) {
        $('#productDetailsFormError').text('Please select the category');
        return;
    }
    addDivTag("", "", "", "", "", "", "", "", "", "");
}

function addDivTag(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10) {
    var divhtml = "<div class='varients-outer' data-index='" + skuDivIndex + "'><input type=checkbox id='check_" + skuDivIndex + "' class='ignore'/></br><span class='text-danger' id='variantsDetailFormSubError'></span><div class='varients-inner'>";

    divhtml += "<input type='text' class='full req' name='Variants[" + skuDivIndex + "].VariantName' id='vname' placeholder='Variant name' value='" + value1 + "'/>"
    divhtml += "<input type='text' class='full' name='Variants[" + skuDivIndex + "].SKU' id='blastname' placeholder='SKU'  value='" + value2 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].OriginalPrice' id='bzip' placeholder='Original Price'  value='" + value3 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].SalePrice' id='bzip' placeholder='Sale Price'  value='" + value4 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].QuantityInStock' id='bzip' placeholder='Quantity in Stock'  value='" + value5 + "'/>"
    divhtml += "<input type='number' class='half req' name='Variants[" + skuDivIndex + "].MinimumInventoryAlert' id='bzip' placeholder='Minimum Inventory Alert'  value='" + value6 + "'/>"
    /*divhtml += "<input type='text' class='half req' name='Variants[" + skuDivIndex + "].Color' id='bzip' placeholder='Color'  value='" + value7     + "'/>"*/
    divhtml += getColorsSelectForVariants(value7, skuDivIndex);
    divhtml += getStyleHtml(value8, skuDivIndex);
    divhtml += getSpecicationAttributesHtml(null, value8, skuDivIndex);
    divhtml += "<textarea type='text' class='full' name='Variants[" + skuDivIndex + "].Description' id='bzip' placeholder='Description' rows='4'  value='" + value10 + "' style='height:auto;'></textarea>"
    divhtml += "<div class='clear'></div>";
    divhtml += "</div ></div >";

    $("#skuDiv").append(divhtml);

    bindClickEvents();
    createSearchSelect(skuDivIndex);
    $('[name="Variants[' + skuDivIndex + '].Description"]').val(value10);
    
    skuDivIndex++;
}

function getStyleHtml(selected, index) {
    let result = '';
    let attr = $.grep(styleAttributes, function (item) {
        return item.categoryId == $categoryIdSelect.val();
    })[0];

    if (attr) {
        if (attr.componentType === 'Dropdown') {
            const ddlValues = attr.values.split(',');

            result += "<select class='half req prod-attribute' id='' name='Variants[" + index + "].Style'>";

            result += "<option value='' " + (selected ? "" : "selected ") + "disabled>" + attr.lable + "</option>"
            for (var i = 0; i < ddlValues.length; i++) {
                if (ddlValues[i] == selected) {
                    result += "<option value='" + ddlValues[i] + "' selected>" + ddlValues[i] + "</option>";
                }
                else {
                    result += "<option value='" + ddlValues[i] + "'>" + ddlValues[i] + "</option>";
                }
            }

            result += "</select>";
        }
        else {
            result += "<input type='text' class='half req prod-attribute' name='Variants[" + index + "].Style' placeholder='" + attr.lable + "'/>"
        }
    }
    
    return result;
}

function getSpecicationAttributesHtml(selected, categoryId, index) {
    let result = '<div class="spec-div"><input type="hidden" class="ignore" id="AttrsJSON" name="Variants[' + index + '].AttrsJSON">';
    let attrs = $.grep(specificationAttributes, function (item) {
        return item.categoryId == $categoryIdSelect.val();
    });

    $.each(attrs, function (i, attr) {
        if (attr.componentType === 'Dropdown') {
            const ddlValues = attr.values.split(',');

            result += "<select class='half req spec-attribute' id='' name='SpecificationAttributes[" + index + "]." + attr.lable + "' data-idmaster='" + attr.id + "'>";

            result += "<option value='' selected disabled>" + attr.lable + "</option>";
            for (var i = 0; i < ddlValues.length; i++) {
                result += "<option value='" + ddlValues[i] + "'>" + ddlValues[i] + "</option>";
            }

            result += "</select>";
        }
        else {
            result += "<input type='text' class='half req spec-attribute' name='SpecificationAttributes[" + index + "]." + attr.lable + "' placeholder='" + attr.lable + "' data-idmaster='" + attr.id + "'/>"
        }
    });

    return result + '</div>';
}

$btnNewImages.click(function () {
    AddImages();
});

function AddImages() {

    if ($('#skuDiv > .varients-outer').length == 0 || !$('#skuDiv').hasClass('variants-saved')) {
        $('#variantsDetailFormError').text('You need to save the variants detail before you can add images for this product.');
        return;
    }
    AddImagesDivTag("");
}

function AddImagesDivTag(value1) {
    var divhtml = "<div class='images-outer'><input type=checkbox id='check_" + imagesDivIndex + "'/><div class='images-inner'><div class=' d-flex'>";

    divhtml += getColorsSelect(null, imagesDivIndex);
    divhtml += "<lable>Thumb Image</lable>"
    divhtml += "<input type='file' class='half req' name='Images[" + imagesDivIndex + "].ThumbImage' id='thumbimage_" + imagesDivIndex + "' placeholder='Thumb Image'/>"
    divhtml += "<lable>Side Images</lable>"
    divhtml += "<input type='file' class='half' name='Images[" + imagesDivIndex + "].SideImages' id='sideimage_" + imagesDivIndex + "' placeholder='Side Images' multiple/></div >"
    divhtml += "<div class='img-upload-show'><div id='display_thumbimg_" + imagesDivIndex + "'></div><div class='sideimg-container' id='display_sideimg_" + imagesDivIndex + "'></div></div>"
    divhtml += "<div class='clear'></div>";
    divhtml += "</div ></div >";

    $("#imagesDiv").append(divhtml);

    bindClickEvents();
    imagesDivIndex++;
}

function getColorsSelectForVariants(selected, index) {
    var selectHtml = "<select class='half req ' id='colourSelect_" + index + "' name='Variants[" + index + "].ColorId'>";

    selectHtml += "<option data-name='' value='' " + (selected ? "" : "selected ") +" disabled>Color</option>";
    for (var i = 0; i < productColours.length; i++) {
        if (productColours[i].id == selected) {
            selectHtml += "<option data-name=" + productColours[i].name + " value='" + productColours[i].id + "' selected>" + productColours[i].name + "</option>";
        }
        else {
            selectHtml += "<option data-name=" + productColours[i].name + " value='" + productColours[i].id + "'>" + productColours[i].name + "</option>";
        }
    }

    selectHtml += "</select>";
    return selectHtml;
}

function getColorsSelect(selected, index) {
    var selectHtml = "<select class='half req ' id='' name='Images[" + index + "].ColourId'>";

    selectHtml += "<option value='' selected disabled>Color</option>";

    $.each(colorsUserSelected, function (i, colorId) {
        var color = productColours.find( function (obj) {
            return obj.id == colorId;
        });
        selectHtml += "<option value='" + color.id + "'>" + color.name + "</option>";
    });

    selectHtml += "</select>";
    return selectHtml;
}

function bindClickEvents() {
    $('.varient-detail [id^="check_"]').click(setBtnVarientCopyDeleteCursor);
    $('.varient-images [id^="check_"]').click(setBtnDeleteImagesCursor);
}

$btnDeleteVarient.live('click', function () {
    var $checkedInputs = $('.varient-detail [id^="check_"]:checked');

    $.each($checkedInputs, function (i, elem) {
        $(this).closest('div.varients-outer').remove();
    });

    setBtnVarientCopyDeleteCursor();
});

$btnDeleteImages.live('click', function () {
    var $checkedInputs = $('.varient-images [id^="check_"]:checked');

    $.each($checkedInputs, function (i, elem) {
        $(this).closest('div.images-outer').remove();
    });

    setBtnDeleteImagesCursor();
});

$btnCopyVarient.live('click', function () {
    //var $checkedInput = $('.varient-detail [id^="check_"]:checked');
    //var $clonedVarient = $('.varient-detail [id^="check_"]:checked').closest('div.varients-outer').clone();
    ////var index = $($('#skuDiv').find('div.varients-outer')[$('#skuDiv').find('div.varients-outer').length - 1]).data('index') + 1;

    //$clonedVarient.data('index', skuDivIndex);
    //$($clonedVarient.find('input[type="checkbox"]')[0]).prop('checked', false)

    //var $checkbox = $clonedVarient.find('input[id^="check_"]');
    //$checkbox.attr('id', $checkbox.attr('id').replace(/check_\d+/, `check_${skuDivIndex}`));

    //var $formFields = $clonedVarient.find('div.varients-inner').find('input,select,textarea');

    //$.each($formFields, function (i, elem) {
    //    $(elem).attr('name', $(elem).attr('name').replace(/\[\d+\]/g, `[${skuDivIndex}]`));
    //});

    //$("#skuDiv").append($clonedVarient);

    //bindClickEvents();
    //skuDivIndex++;

    const $variantTobeCloned = $('.varient-detail [id^="check_"]:checked').closest('div.varients-outer').children('div.varients-inner');
    if ($variantTobeCloned.length > 0) {
        var variantFormData = getSerializedObject($variantTobeCloned);
        addDivTag(variantFormData.VariantName, variantFormData.SKU, variantFormData.OriginalPrice, variantFormData.SalePrice
            , variantFormData.QuantityInStock, variantFormData.MinimumInventoryAlert, variantFormData.ColorId, variantFormData.Style
            , null, variantFormData.Description);
    }
    return;
});

function getSerializedObject($formContainer) {
    const $formFields = $formContainer.find('input,select,textarea');
    let data = {};
    $.each($formFields, function (i, elem) {
        const fieldName = $(elem).attr('name');
        data[fieldName.split('.')[1]] = $(elem).val();
    });
    return data;
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

            img.onload = function () {;
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

$categoryIdSelect.live('change', function () {
    $that = $(this);
    $('#productDetailsFormError').text('');
    let varientDetailForms = $("#skuDiv").find('div.varients-outer');
    if (varientDetailForms.length) {
        
        $.each(varientDetailForms, function (index, div) {
            var index = $(div).data('index');

            const styleHtml = getStyleHtml(null, index);
            $(div).find('select.prod-attribute').replaceWith(styleHtml);

            const specificationAttributesHtml = getSpecicationAttributesHtml(null, $that.val(), index);
            $(div).find('div.spec-div').replaceWith(specificationAttributesHtml);
        });
    }

    //set hidden fields value
    var selectedOption = $(this).find("option:selected");
    $("#CategoryName").val(selectedOption.data("name"));
    $("#ParentCategoryName").val(selectedOption.data("parent-name"));
});

$btnSaveVarient.click(function () {

    let saveSuccess = true;
    const forms = $('#skuDiv').find('.varients-outer');
    $.each(forms, function (i, form) {
        const errorSpan = $(form).children('#variantsDetailFormSubError');
        errorSpan.text('');

        const formElems = $(form).find('input:not(.ignore), select, textarea');
        $.each(formElems, function (i, elem) {
            if ($(elem).val() == '') {
                saveSuccess = false;
                errorSpan.text('Please fill all the details');
                return false;
            }
        });
    });

    if (saveSuccess) {
        $('#skuDiv').find('.varients-outer').find('input,select,textarea').attr('disabled', true);
        $('#skuDiv').addClass('variants-saved');
        //$('#variantsDetailFormError').text('');

        var colorSelectElems = $('#skuDiv .varients-outer').find('[id^="colourSelect_"]');
        $.each(colorSelectElems, function (i, select) {
            colorsUserSelected.push($(select).val());
        });
        colorsUserSelected = Array.from(new Set( // Will return unique values
            colorSelectElems.map(function (i, elem) {
                return $(elem).val();
            })
            .get()
        ))
    }
});

$("#btnSkuFormSubmit").click(function () {
    $('#skuDiv').find('.varients-outer').find('input,select,textarea').attr('disabled', false);

    // Set Spec-attrs's JSON Value
    const varientForms = $('#skuDiv').find('.varients-inner');
    $.each(varientForms, function (i, elem) {
        const inp = $(elem).find('input#AttrsJSON');

        let result = [];
        const attrInps = inp.closest('.spec-div').find('.spec-attribute');
        $.each(attrInps, function (i, bElem) {
            let obj = {}
            obj.idmaster = $(bElem).data('idmaster');
            obj.attrval = $(bElem).val().trim();
            result.push(obj)
        })

        inp.val(JSON.stringify(result));
    })

    $(".form-sku").submit();
});



//////////////////////////////////////////////////////////            Custom Dropdown              ////////////////////////////////////////////////////////////////////

function createSearchSelect(index) {
    // Get dropdown(s)
    var dropdowns = $('#skuDiv').find('#colourSelect_' + index);

    // Check if dropdowns exist on page
    if (dropdowns.length > 0) {
        // Loop through dropdowns and create custom dropdown for each select element
        dropdowns.each(function () {
            createCustomDropdown($(this));
        });
    }

    // Create custom dropdown
    function createCustomDropdown(dropdown) {
        // Get all options and convert them from nodelist to array
        var options = dropdown.find('option');
        var optionsArr = options.toArray();

        // Create custom dropdown element and add class dropdown to it
        // Insert it in the DOM after the select field
        var customDropdown = $('<div class="dropdown"></div>');
        dropdown.after(customDropdown);

        // Create element for selected option
        // Add class to this element, text from the first option in select field and append it to custom dropdown
        var selected = $('<div class="dropdown__selected" id="dropdown__selected__' + index + '"></div>');
        selected.text(
            dropdown.val()
                ? productColours.find(function (obj) {
                    return obj.id == dropdown.val();
                }).name
                : "Color"
        );
        customDropdown.append(selected);

        // Create element for dropdown menu, add class to it and append it to custom dropdown
        // Add click event to selected element to toggle dropdown menu
        var menu = $('<div class="dropdown__menu" id="dropdown__menu__' + index + '"></div>');
        customDropdown.append(menu);

        selected.click(function () {
            if (menu.is(':visible')) {
                menu.hide();
            } else {
                menu.show();
                menu.find('input').focus();
            }
        });

        // Create serach input element
        // Add class, type and placeholder to this element and append it to menu element
        var search = $('<input class="dropdown__menu_search ignore" name="" type="text" placeholder="Search...">');
        var searchSpan = $('<span class="dropdown__menu_searchSpan"></span>');
        searchSpan.append(search);
        menu.append(searchSpan);

        // Create wrapper element for menu items, add class to it and append to menu element
        var menuItemsWrapper = $('<div class="dropdown__menu_items"></div>');
        menu.append(menuItemsWrapper);

        // Loop through all options and create custom option for each option and append it to items wrapper element
        // Add click event for each custom option to set clicked option as selected option
        optionsArr.forEach(function (option) {
            var item = $('<div class="dropdown__menu_item" data-value="' + option.value + '"></div>');
            item.text(option.textContent);
            menuItemsWrapper.append(item);

            item.click(function () {
                setSelected(selected, dropdown, menu, $(this));
            });
        });

        // Add selected class to first custom option
        //menuItemsWrapper.find('div').first().addClass('selected');
        menuItemsWrapper.find('div').first().addClass('selected');

        // Add input event to search input element to filter items
        search.keyup(function () {
            filterItems(optionsArr, menu, search);
        });

        // Add click event to document element to close custom dropdown if clicked outside of it
        $(document).click(function (e) {
            if ($(e.target).closest('.dropdown').length === 0 && e.target !== customDropdown && menu.is(':visible')) {
                menu.hide();
            }
        });

        // Hide original dropdown(select)
        dropdown.hide();
    }

    function setSelected(selected, dropdown, menu, item) {
        var value = item.data('value');
        var label = item.text();

        selected.text(label);
        dropdown.val(value);

        menu.hide();
        menu.find('input').val('');
        menu.find('div').each(function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            if ($(this).is(':hidden')) {
                $(this).show();
            }
        });
        item.addClass('selected');
    }

    function filterItems(itemsArr, menu, search) {
        var customOptions = menu.find('.dropdown__menu_items div');
        var value = search.val().toLowerCase();
        var filteredItems = itemsArr.filter(function (option) {
            return option.dataset.name.toLowerCase().includes(value);
        });
        var indexesArr = filteredItems.map(function (option) {
            return itemsArr.indexOf(option);
        });

        itemsArr.forEach(function (option) {
            if (!indexesArr.includes(itemsArr.indexOf(option))) {
                customOptions.eq(itemsArr.indexOf(option)).hide();
            } else {
                if (customOptions.eq(itemsArr.indexOf(option)).is(':hidden')) {
                    customOptions.eq(itemsArr.indexOf(option)).show();
                }
            }
        });
    }
}


