$(".delete-comment-btn").click(confirmAction)

function confirmAction() {
    if (confirm('Do you really want to delete the selected comment?')) {
        let parent = $(this).parent('.comment');

        console.log($(this).attr('data-commentId'));

        $.ajax({
            url: "/Car/DeleteComment",
            data: {
                commentId: $(this).attr('data-commentId')
            },
            type: "Post",
            dataType: "Json"
        });

        parent.remove();

        return true;
    } else {
        return false;
    }
}