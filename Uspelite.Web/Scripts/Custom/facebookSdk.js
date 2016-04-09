$(document).ready(function(){
	window.fbAsyncInit = function() {
    FB.init({
        appId: '1621499054772597',
      xfbml      : true,
      version    : 'v2.5'
    });
	
	FB.api(
	  '/370333769731136',
	  'GET',
	  {"fields":"likes"},
	  function(response) {
		  console.log(response);
	  }
	);
  };
  
  

  (function(d, s, id){
     var js, fjs = d.getElementsByTagName(s)[0];
     if (d.getElementById(id)) {return;}
     js = d.createElement(s); js.id = id;
     js.src = "//connect.facebook.net/en_EN/sdk.js";
     fjs.parentNode.insertBefore(js, fjs);
   }(document, 'script', 'facebook-jssdk'));
});