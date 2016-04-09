$((function () {

    var articleSlug = $('#article-post').attr('data-slug')
    var current_uri = 'http://uspelite.com/' + articleSlug;

    $.when(getFacebookSharesCount(), getTwitterSharesCount(), getGooglePlusSharesCount(), getLinkedInSharesCount()).done(function (fb, tw, gp, li) {
        var fbShares = fb[0].shares,
             liShares = li[0].result.metadata.globalCounts.count,
             twShares = tw[0].count,
             gpShares = gp[0].count,
             totalSharesCount = fbShares + liShares + twShares + gpShares;

        updateShareButton($('.ssb_facebook_count'), fbShares);
        updateShareButton($('.ssb_linkedin_count'), liShares);
        updateShareButton($('.ssb_twitter_count'), twShares);
        updateShareButton($('.ssb_google_count', gpShares));

        updateTotalShares(totalSharesCount);
    });

    function getFacebookSharesCount() {
        return $.getJSON('https://graph.facebook.com/' + current_uri);
    }

    function getTwitterSharesCount() {
        return $.getJSON('https://www.linkedin.com/countserv/count/share?url=' + current_uri + '&callback=?');
    }

    function getGooglePlusSharesCount() {
        return $.getJSON('http://opensharecount.com/count.json?url=' + current_uri);
    }

    function getLinkedInSharesCount() {
        return $.ajax({
            type: 'POST',
            url: 'https://clients6.google.com/rpc',
            processData: true,
            contentType: 'application/json',
            data: JSON.stringify({
                'method': 'pos.plusones.get',
                'id': current_uri,
                'params': {
                    'nolog': true,
                    'id': current_uri,
                    'source': 'widget',
                    'userId': '@viewer',
                    'groupId': '@self'
                },
                'jsonrpc': '2.0',
                'key': 'p',
                'apiVersion': 'v1'
            })
        });
    }



    function updateShareButton($element, shares) {
        debugger;
        if (shares == 0) {
            return;
        }

        $element.text(shares);
    }

    function updateTotalShares(totalSharesCount) {
        $('.ssb_t_nb').text(totalSharesCount);
    };
}()));
