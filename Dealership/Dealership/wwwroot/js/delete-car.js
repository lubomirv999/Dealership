$('.delete-car-all-btn').click(confirmAction);

function confirmAction() {
    if (confirm('Do you really want to delete the car?')) {
        return true;
    } else {
        return false;
    }
}