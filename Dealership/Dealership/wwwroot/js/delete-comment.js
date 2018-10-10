$(".delete-comment-btn").click(confirmAction)

function confirmAction() {
    if (confirm('Do you really want to delete the selected comment?')) {
        let parent = $(this).parent('.author').parent('.comment');
        let child = $(this).parent('.replier').parent('.reply');

        $.ajax({
            url: "/Car/DeleteComment",
            data: {
                commentId: $(this).attr('data-commentId')
            },
            type: "Post",
            dataType: "Json"
        });

        parent.remove();
        child.remove();

        return true;
    } else {
        return false;
    }
}