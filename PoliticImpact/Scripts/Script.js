function showAndHideVotingTitle() {
    if ($("#create_voting").val() == "yes") {
        jQuery("#voting_input").slideDown();
    }
    else {
        jQuery("#voting_input").slideUp();
    }
}