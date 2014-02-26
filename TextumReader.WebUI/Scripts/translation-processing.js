/*
function TranslationPrecessing(params) {
    this.getTranslationAction = params.getTranslationAction;
    this.addWordAction = params.addWordAction;
    this.getSavedTranslationsAction = params.getSavedTranslationsAction;
    this.dialogId = params.dialogId;
    this.wordTagName = params.wordTagName;
    this.translationTagName = params.translationTagName;
    this.dictionaryId = params.dictionaryId;
    
    this.castomizeDialog = function () {
        $(this.dialogId).dialog({
            autoOpen: false,
            resizable: false,
            width: 200,
            height: 200
        });
    };

    this.formatTranslation = function (translation) {
        var formattedData =
            "<" + this.translationTagName + ">" +
                translation +
                "</" + this.translationTagName + ">" + "</br>";

        return formattedData;
    };
   

    this.establish = function () {
        $(this.wordTagName).click(function (e) {
            var selectedWord = $(this).text();

            $.post(this.getTranslationAction, { word: selectedWord }, function (data) {
                $.post(this.getSavedTranslationsAction, { word: selectedWord, dictionaryId: this.dictionaryId }, function (savedTranslations) {
                    var formattedData = ""; // Data with translations

                    for (var i = 0; i < savedTranslations.length; i++) {
                        formattedData += "✔ " + this.formatTranslation(savedTranslations[i]);
                    }

                    for (var i = 0; i < data.Translations.length; i++) {
                        if (savedTranslations.indexOf(data.Translations[i]) == -1) {
                            formattedData += this.formatTranslation(data.Translations[i]);
                        }
                    }

                    $(dialogId).html(formattedData);

                    // highlights translations by adding class translationLight
                    highlightWords("translation", "translationLight");

                    $(this.translationTagName).click(function (e) {
                        var selectedTranslation = $(this).text();
                        $.post(this.addWordAction, {
                            word: selectedWord,
                            translation: selectedTranslation,
                            dictionaryId: this.dictionaryId
                        }, function (data) {
                            if (data) {
                                throw "Haven't get data from " + urlToAction;
                            } else {
                                $(this.dialogId).dialog("close");
                                //$(this).html(formatTranslation(selectedTranslation));
                            }
                        });
                    });

                    // сalculates proper coordinates for the dialog 
                    var x = e.pageX - $(document).scrollLeft();
                    var y = e.pageY - $(document).scrollTop();

                    $(this.dialogId).dialog("option", { position: [x - 100, y + 14] });

                    $(this.dialogId).dialog("option", "title", data.WordName).dialog("open");
                });
            });
        });
    };
}
*/