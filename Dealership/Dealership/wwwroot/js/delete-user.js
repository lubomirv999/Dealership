$('.delete-user-all-btn').click(confirmAction);

function confirmAction() {
    if (confirm('Do you really want to delete the user?')) {
        return true;
    } else {
        return false;
    }
}