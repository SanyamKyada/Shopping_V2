$(document).ready(function () {


    function mycarousel_initCallback(carousel) {
        jQuery('.cnext').bind('click', function () {
            carousel.next();
            return false;
        });

        jQuery('.cprev').bind('click', function () {
            carousel.prev();
            return false;
        });
    };


    jQuery('.carousels').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback,
        // This tells jCarousel NOT to autobuild prev/next buttons
        buttonNextHTML: null,
        buttonPrevHTML: null
    });
    //search
    $(".menu1 li").hover(
        function () {
            $(this).find("div").fadeIn();
        },
        function () {
            $(this).find("div").fadeOut();
        }
    );


    $(".cartstatus").click(function () {
        $(".cartdrop").fadeToggle(300);
        $(".logreg").hide();
        if (!$(this).hasClass("cartstatus2")) { $(this).addClass("cartstatus2"); }
        else { $(this).removeClass("cartstatus2"); }
    });
    $(".loginregister").click(function () {
        $(".logreg").fadeToggle(300);
        $(".cartdrop").hide();
        if (!$(this).hasClass("loginregisteractive")) { $(this).addClass("loginregisteractive"); }
        else { $(this).removeClass("loginregisteractive"); }
    });

    $(".search").click(function () {
        $('.searchtoggle').toggle('fast', function () {
        });
    });
    $(".slideprev").click(function () {
        var udaljeno = $(".sliderinner ul").css("left");
        if (udaljeno != '0px') {
            $('.sliderinner ul').animate({ left: '+=940' });
            var palica = $(".handle").css("left");
            if (palica == '195px') {
                $(".handle").animate({ left: '0' });
            }
            if (palica == '380px') {
                $(".handle").animate({ left: '195' });
            }
            if (palica == '570px') {
                $(".handle").animate({ left: '380' });
            }
            if (palica == '760px') {
                $(".handle").animate({ left: '570' });
            }
        }
    });
    //desna
    $(".slidenext").click(function () {
        var udaljeno = $(".sliderinner ul").css("left");
        if (udaljeno != '-3760px') {
            $('.sliderinner ul').animate({ left: '-=940' });
            var palica = $(".handle").css("left");
            if (palica == '0px') {
                $(".handle").animate({ left: '195' });
            }
            if (palica == '195px') {
                $(".handle").animate({ left: '380' });
            }
            if (palica == '380px') {
                $(".handle").animate({ left: '570' });
            }
            if (palica == '570px') {
                $(".handle").animate({ left: '760' });
            }
        }
    });
    $('.googleinner').gMap({
        markers: [{
            latitude: 47.660937,
            longitude: 9.569803,
            html: "Here we are!",
            popup: true
        }],
        zoom: 6
    });
    //gridswitch
    $('.gridtype').click(function () {
        $('.griditems').hide();
        $('.grayprod').show();
    });
    $('.listtype').click(function () {
        $('.grayprod').hide();
        $('.griditems').show();
    });
    $('input.star').rating();
    //slider 2
    $('.slider2 ul').anythingSlider({
        navigationFormatter: function (index, panel) {
            return " " + index; // This would have each tab with the text 'Panel #X' where X = index
        },
        height: 381,
        hashtags: false,
        buildArrows: false
    });
    var widthslide = $('.thumbNav').width();
    var wholewidth = 940 - widthslide;
    wholewidth = wholewidth / 2;
    $('.thumbNav').css('margin-left', wholewidth);
    //tabs
    var tabContainers = $('div.tabs > div');
    tabContainers.hide().filter(':first').show();

    $('div.tabs ul.tabNavigation a').click(function () {
        tabContainers.hide();
        tabContainers.filter(this.hash).show();
        $('div.tabs ul.tabNavigation li').removeClass('ewizz');
        $(this).parent().addClass('ewizz');
        return false;
    }).filter(':first').click();
});


/*-------------------------------------------------------------------------------------------------------------------------------|
|                                                Custom Tags-input (jQuery prototype)                                            |
|<------------------------------------------------------------------------------------------------------------------------------*/

(function ($) {
    $.fn.AddTags = function (options) {
        // Default options
        var settings = $.extend({
            maxTags: 10,
            initialTags: [],
            includeTitle: true // You can set this to false if you don't want the title
        }, options);

        return this.each(function () {
            var inputElement = $(this);
            var wrapper = $("<div class='wrapper'></div>"); //class=wrapper
            var content = $("<div class='content'></div>");
            var instructions = $("<p>Press enter or add a comma after each value</p>");
            var ul = $("<ul id='tagList'></ul>");
            var tagInput = inputElement.clone();
            var details = $("<div class='details'></div>");
            var tagRemaining = $("<p><span id='tagRemaining'></span> tags are remaining</p>");
            var removeAllBtn = $("<button id='removeAllTags'>Remove All</button>");

            if (settings.includeTitle) {
                var title = $("<div class='title'><h2>Tags</h2></div>");
                content.append(title);
            }

            // Append elements to create the structure
            ul.append(tagInput);
            content.append(instructions, ul);
            details.append(tagRemaining, removeAllBtn);
            wrapper.append(content, details);
            inputElement.replaceWith(wrapper);

            var maxTags = settings.maxTags;
            var tags = settings.initialTags || [];

            createTag();

            function countTags() {
                tagInput.focus();
                tagRemaining.children().text(maxTags - tags.length);

                if (tags.length == 0) {
                    removeAllBtn.prop("disabled", true);
                    removeAllBtn.css('cursor', 'no-drop');
                } else {
                    removeAllBtn.prop("disabled", false);
                    removeAllBtn.css('cursor', '');
                }
            }

            function createTag() {
                ul.find("li").remove();
                tags.slice().reverse().forEach(tag => {
                    let liTag = `<li>${tag} <i class="fa fa-close remove-tag"></i></li>`;
                    ul.prepend(liTag);
                });
                bindRemoveClickEvent();
                countTags();
            }

            function bindRemoveClickEvent() {
                $(".remove-tag").click(function () {
                    let tag = $(this).parent().text().trim();
                    $(this).parent().remove();
                    let index = tags.indexOf(tag);
                    tags.splice(index, 1);
                    countTags();
                });
            }

            function addTag(e) {
                if (e.keyCode === 13/* || e.keyCode === 188*/) {
                    let tag = e.target.value.trim();
                    if (tag.length > 0 && !tags.includes(tag)) {
                        if (tags.length < maxTags) {
                            tag.split(',').forEach(tag => {
                                tags.push(tag);
                            });
                            createTag();
                        }
                    }
                    e.target.value = "";
                }
            }

            tagInput.keyup(addTag);

            removeAllBtn.click(function () {
                tags = [];
                createTag();
            });
        });
    };
})(jQuery);
