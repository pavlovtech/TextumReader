function wrapAllWords(containerId, tagName) {
    var text = $(containerId).text();

    text = text.replace(/[a-zA-z’'-]+/g, "<" + tagName + ">$&</" + tagName + ">");
    text = text.replace(/\n/g, "<br />");
    $(containerId).html(text);
}
//# sourceMappingURL=word-splitter.js.map
