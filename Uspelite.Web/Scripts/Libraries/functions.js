(function($) {
    "use strict";

    // Navigation slide down
    $("#menu .nav li").hover(function(){
        $(this).find('.dropnav-container, .subnav-container').slideDown(300);
    },function(){
        $(this).find('.dropnav-container, .subnav-container').hide();
    });

    // Subnav article loader
    $('#menu .subnav-menu li').hover(function() {
        $(this).parent().find('li').removeClass('current');
        $(this).addClass('current');
    });

    // Navigation Menu Expander
    $('#nav-expander').sidr({
        side: 'right'
    });

    $('#sidr li a.more').click(function(e) {
        e.preventDefault();
        $(this).parent().find('ul').toggle();
    });

    //// Homepage slider
    $('#slider-carousel').carouFredSel({
        width: '100%',
        height: 'variable',
        //prev: '#slider-prev',
        //next: '#slider-next',
        responsive: true,
        transition: true,
        items: {
            height: 'variable'
        },
        //swipe: {
        //    onMouse: true,
        //    onTouch: true
        //},
        //scroll : {
        //    items           : 1,
        //    easing          : "quadratic",
        //    duration        : 1000,
        //    pauseOnHover    : true
        //},
        auto : false,
        onCreate: function () {
            $(window).on('resize', function () {
                var carousel = $('#slider-carousel');
                carousel.parent().add(carousel).height(carousel.children().first().height()+'px');
            }).trigger('resize');
        }
    }).trigger('resize');

    //$('#slider-carousel').swipe({
    //    tap:function (event, target) {
    //        $(target).click();
    //    },
    //    swipe:function(event, direction, distance, duration, fingerCount) {
    //    },
    //    threshold:50
    //});

    // Init photobox
    $('#weekly-gallery').photobox('a',{ time:0 });
    $('#article-gallery').photobox('a',{ time:0 });

    // Init datepicker for archive page
    $('#archive-date-picker').datepicker({
        format: 'mm/dd/yyyy'
    });

    //Click event to scroll to top
    $('.scrollToTop').click(function(){
        $('html, body').animate({scrollTop : 0},800);
        return false;
    });


})(jQuery);
