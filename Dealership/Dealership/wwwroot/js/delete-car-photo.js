$(".delete-car-all-btn").click(confirmAction)

function confirmAction() {
    if (confirm('Do you really want to delete the selected photo?')) {
        let parent = $(this).parent('.image-ctr');

        $.ajax({
            url: "/Car/DeletePhoto",
            data: {
                photoId: $(this).attr('data-photoId')
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