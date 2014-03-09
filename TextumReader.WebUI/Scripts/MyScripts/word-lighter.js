function highlightWords(tagName, className) {
    var words = $(tagName).hover(function (eventObject) {
        $(this).addClass(className);
    }).mouseout(function () {
        $(this).removeClass(className);
    });
}