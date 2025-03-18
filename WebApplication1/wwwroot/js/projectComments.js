function loadComments(projectId){
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId'+ projectId,
        method: "GET",
        success: function(data){
            var commentsHtml = '';
            for(var i = 0; i<data.length; i++){
                commentsHtml += '<div class="comments">';
                commentsHtml += '<p> + data[i.content] + '</p>';
                commentsHtml += '<span> Posted on: new Date(data[i].datePosted).toLocalDateString() + '</span>';
                commentsHtml += '</div>';
            }
            $('commentsList').html(commentsHtml);
        }
    });
}