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
    
}