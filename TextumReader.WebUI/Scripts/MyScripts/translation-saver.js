/*
function saveTranslation(urlToAction, selectedWord, tagName, dictId) {
    $(tagName).click(function (e) {
        var selectedTranslation = $(this).text();
        $.post(urlToAction, { word: selectedWord, translation: selectedTranslation, dictionaryId: dictId }, function (data) {
            if (data) {
                throw "Haven't get data from " + urlToAction;
            }
        });
    });
}
*/