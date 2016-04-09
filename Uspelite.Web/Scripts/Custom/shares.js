$(document).ready(function(){
	
	var current_uri = 'http://uspelite.com/martin-angelov-2';
	
	updateTotalShares = function(newShares) {
		currentShares = Number($('.ssb_t_nb').text());
		
		newShares += currentShares;
		
		$('.ssb_t_nb').text(newShares);
	};
	
	$.getJSON('https://graph.facebook.com/' + current_uri, function (fbdata) {
		fbShares = fbdata.shares;
		if (fbShares == 0) {
			return;
		}
		$('.ssb_facebook_count').text(fbShares);
		
		updateTotalShares(fbShares);
	});
	
	$.getJSON('https://www.linkedin.com/countserv/count/share?url=' + current_uri + '&callback=?', function (lidata) {
		liShares = lidata.count;
		if (liShares == 0) {
			return;
		}
		$('.ssb_linkedin_count').text(liShares);
		
		updateTotalShares(liShares);
	});
	
	$.getJSON('http://opensharecount.com/count.json?url=' + current_uri, function(twdata) {
		twShares = twdata.count;
		if (twShares == 0) {
			return;
		}
		$('.ssb_twitter_count').text(twShares);
		
		updateTotalShares(twShares);
	});
	
	$.ajax({
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
	  }),
	  success: function(response) {
		gpShares = response.result.metadata.globalCounts.count;
		if (gpShares == 0) {
			return;
		}
		$('.ssb_google_count').text(gpShares);
		
		updateTotalShares(gpShares);
	  }
	});
	
	FB.api(
	  '/370333769731136',
	  'GET',
	  {"fields":"likes"},
	  function(response) {
		  // Insert your code here
	  }
	);
});
