function showAndHideVotingTitle() {
    if ($("#create_voting").val() == "yes") {
        jQuery("#voting_input").slideDown();
    }
    else {
        jQuery("#voting_input").slideUp();
    }
}

//Added by Johannes Dahlgren 20/11 2012
function deleteCaseItem(id) {
    if (confirm("Do you want to delete?")) {
        jQuery.ajax({
            type: "POST",
            url: "/CaseItems/Delete/" + id,
            datatype: "json"
        }).done(function() {
            
            window.location = "/CaseItems/Index";
        });
    }
    else {
        jQuery.ajax({
            type: "POST", url: "/CaseItems/Details/" + id
        });
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
>>>>>>> f443870b552e45e355eb73f7d4b3ca7deb4f3efa
}