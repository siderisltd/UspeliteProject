$(function () {
    $('div.review-rating ul').mouseover(function() {
        $(this).css('cursor', 'pointer');
    });

    $('div.review-rating ul li').bind({
        mouseover: function () {
            var $hoveredElement = $(this),
                previousSiblings = $hoveredElement.prevAll('li'),
                nextSiblings = $hoveredElement.nextAll('li'),
                rateAmmount = previousSiblings.length + 1;

            $hoveredElement.addClass("li-rated");

            previousSiblings.each(function (it) {
                var item = previousSiblings[it];
                item.className = "li-rated";
            });

            nextSiblings.each(function (it) {
                var item = nextSiblings[it];
                if (item !== undefined) {
                    item.className = "";
                }
            });
        },
        click: function () {
            var $clickedElement = $(this),
                parentUl = $clickedElement.parent('ul'),
                previousSiblingsLength = $clickedElement.prevAll('li').length,
                ratePoints = previousSiblingsLength + 1,
                rateId = parentUl.attr('data-Id'),
                rateType = parentUl.attr('data-rateType');

            var data = {
                rateId: rateId, 
                ratePoints: ratePoints,
                rateType : rateType
            }

            $.ajax({
                type: 'POST',
                url: '/Ratings/Rate',
                data: data,
                complete: function (res) {
                    var responseObject = JSON.parse(res.responseText);
                    var rateMessageBox = parentUl.next('div.rate-message');
                    rateMessageBox.html(responseObject.message);
                    rateMessageBox.css('display', 'block');
                    rateMessageBox.css('position', 'absolute');
                    rateMessageBox.css('z-index', '100000000');

                    rateMessageBox.delay(2700).fadeOut(100);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
    });


    $('div.review-rating ul').on('mouseleave', function () {
        var $rateElement = $(this),
            previousRating = $rateElement.attr('data-currentRating');

        var children = $rateElement.children('li');

        for (var i = 0; i < children.length; i++) {
            var currentChild = children[i];

            if (previousRating > i) {
                currentChild.className = "li-rated";
            } else {
                currentChild.className = "";
            }
        }
    });
});