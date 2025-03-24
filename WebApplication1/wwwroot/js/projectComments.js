function loadComments(projectId){
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
        method: "GET",
        success: function(data){
            var commentsHtml = '';
            for(var i = 0; i < data.length; i++){
                var datePosted = new Date(data[i].datePosted);
                var formattedDate = isNaN(datePosted.getTime()) ? "Invalid Date" : datePosted.toLocaleString();
                commentsHtml += '<div class="comments">';
                commentsHtml += '<p>' + (data[i].content || "No content provided") + '</p>';
                commentsHtml += '<span><b>Posted on:</b><i>' + formattedDate + '</i></span>';
                commentsHtml += '</div>';
            }
            $('#commentsList').html(commentsHtml);
        },
        error: function(error) {
            console.log('Error loading comments:', error);
        }
    });
}

$(document).ready(function() {
    var projectId = $('#projectComments input[name="ProjectId"]').val();
    loadComments(projectId);
    
    //Submit event for new comment (AddComment)
    $('#addCommentForm').submit(function (evt){
        
        evt.preventDefault();
        
        var formData = {
            projectId: projectId,
            Content: $('#projectComments textarea[name="Content"]').val()};
        
        
        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if(response.success){
                    $('#projectComments textarea[name="Content"]').val('') //clear new comment from form textarea
                    loadComments(projectId); //reload comments after adding a new one
                }else{
                    alert(response.message);
                }
            },
            error: function(xhr, status, error){
                alert("Error: " + xhr.responseText);
            }
        });
        
    });
    
});