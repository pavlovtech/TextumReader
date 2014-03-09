function submitFormAutomatically (id) {
    $(id).change(function () {
        $(this).parents('form:first').find(':submit')[0].click();
    });
}