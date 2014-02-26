function formatTranslation(translationTagName, translation) {
    var formattedData =
        "<" + translationTagName + ">" +
        translation +
        "</" + translationTagName + ">" + "</br>";

    return formattedData;
}

function setDialog(params) {
    $(params.dialogId).dialog({
        autoOpen: false,
        resizable: false,
        width: 200,
        height: 200
    });

    $(params.wordTagName).click(function (e) {
        var selectedWord = $(this).text().toLocaleLowerCase();
        
        $.post(params.getTranslationAction, { word: selectedWord }, function (data) {
            $.post(params.getSavedTranslationsAction, { word: selectedWord, dictionaryId: params.dictionaryId }, function (savedTranslations) {
                var formattedData = ""; // Data with translations

                for (var i = 0; i < savedTranslations.length; i++) {
                    formattedData += "✔ " + formatTranslation(params.translationTagName, savedTranslations[i]);
                }
                
                for (var i = 0; i < data.Translations.length; i++) {
                    if (savedTranslations.indexOf(data.Translations[i]) == -1) {
                        formattedData += formatTranslation(params.translationTagName, data.Translations[i]);
                    }
                }

                $(params.dialogId).html(formattedData);
                
                // highlights translations by adding class translationLight
                highlightWords("translation", "translationLight");

                $(params.translationTagName).click(function (e) {
                    var selectedTranslation = $(this).text();
                    $.post(params.addWordAction, {
                        word: selectedWord,
                        translation: selectedTranslation,
                        dictionaryId: params.dictionaryId
                    }, function (data) {
                        if (data) {
                            throw "Haven't get data from " + urlToAction;
                        } else {
                            $(params.dialogId).dialog("close");
                            //$(this).html(formatTranslation(selectedTranslation));
                        }
                    });
                });

                // сalculates proper coordinates for the dialog 
                var x = e.pageX - $(document).scrollLeft();
                var y = e.pageY - $(document).scrollTop();

                $(params.dialogId).dialog("option", { position: [x - 100, y + 14] });

                $(params.dialogId).dialog("option", "title", data.WordName).dialog("open");
            });
        });
    });
}