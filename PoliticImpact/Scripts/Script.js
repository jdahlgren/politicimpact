function showAndHideVotingTitle() {
    if ($("#create_voting").val() == "yes") {
        jQuery("#voting_input").slideDown();
    }
    else {
        jQuery("#voting_input").slideUp();
    }
}


function SendVote(id) {
    var strVote = jQuery("input:radio[name=voting-choice]").val();
    jQuery.ajax({
        type: "POST",
        url: "/CaseVotes/Create/" + id,
        data: { Vote:  strVote}
    }).done(function (msg) {
        jQuery('#voting_container').slideUp();
        jQuery('#voting_container').after("<p>You voted " + strVote + "</p>");
        console.log("voting sent");
    });
}


function ShareCaseByEmail(id) {
    jQuery.ajax({
        type: "POST",
        url: "/CaseItems/ShareMail/" + id,
        data: { email: jQuery('#friend_email').val() }
    }).done(function (msg) {
        jQuery('#share_by_email').html("<p>Email sent</p>");
        jQuery('#friend_email').val("");
    });
}