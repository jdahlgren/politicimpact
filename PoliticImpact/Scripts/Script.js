function showAndHideVotingTitle() {
    if ($("#create_voting").val() == "yes") {
        jQuery("#voting_input").slideDown();
    }
    else {
        jQuery("#voting_input").slideUp();
    }
}


function deleteCaseItem(id) {
    if (confirm("Vill du ta bort förslaget?")) {
        jQuery.ajax({
            type: "POST",
            url: "/CaseItems/Delete/" + id,
            datatype: "json"
        }).done(function () {

            window.location = "/CaseItems/Index";
        });
    }
    else {
        jQuery.ajax({
            type: "POST", url: "/CaseItems/Details/" + id
        });
    }
}


function SendVote(id) {
    var strVote = jQuery("input:radio[name=voting-choice]:checked").val();
    jQuery.ajax({
        type: "POST",
        url: "/CaseVotes/Create/" + id,
        data: { Vote: strVote }
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

function PostComment(id) {
    var strComment = jQuery("#newCommentStr").val();
    jQuery.ajax({
        type: "POST",
        url: "/CaseComments/Create/" + id,
        data: { comment: strComment }
    }).done(function (msg) {
        console.log("comment created");
    }).always(function () {
        $("#newComment").after("<p>" + strComment + "</p>");
        $("#newComment").slideUp();
        $("#newCommentStr").val("");
        $("#nocomments").remove();
    });
}

function ShowCommentField() {
    $("#fiveComments").hide();
    $("#allComments").show();
    $("#newComment").show();
}

/**
*function ArchiveCaseItem(id)
*en funktion som skickar ett POSTrequest till /CaseItems/Archive/id 
*där controllerns action "Archive" anropas.
*/
function ArchiveCaseItem(id) {
    jQuery.ajax({
        type: "POST",
        url: "/CaseItems/Archive/" + id,
    }).done(function (msg) {
        jQuery('#archived_status').html('<p>Arkiverad, går inte längre att interagera.</p>');
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