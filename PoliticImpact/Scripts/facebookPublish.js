function publishStory(name, description, page) {
    FB.ui({
        method: 'feed',
        name: name, //This should be the subject of the case
        //caption: 'Hope this works', 
        description: description, //This should be the discription of the case
        link: page, //This should be the page I'm on, the page of the case
        picture: 'http://politicimpact.azurewebsites.net/Content/Images/icon.png' // This should be the image of the case
    },
    function (response) {
        console.log('publishStory response: ', response);
    });
    return false;
}
/*@Scripts.Render("~/Scripts/facebookPublish.js")
@Scripts.Render("~/Scripts/facebookConnect.js")
<a href="#" onclick="publishStory();">Publish feed story</a><br>*/