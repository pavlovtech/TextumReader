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
        width: 200
    });

    $(params.wordTagName).click(function(e) {
        var currentDict = $("#DictionaryId option:selected").attr("value");

        var selectedWord = $(this).text().toLocaleLowerCase();
        var wordName;
        var wordFrequency;

        $.post(params.getSavedTranslationsAction, { word: selectedWord, dictionaryId: currentDict }, function(savedTranslations) {
            var formattedData = ""; // Data with translations

            for (var i = 0; i < savedTranslations.length; i++) {
                formattedData += "✔ " + formatTranslation(params.translationTagName, savedTranslations[i]);
            }

            if (navigator.onLine == true) {
                $.ajaxSetup({ async: false });
                $.post(params.getTranslationAction, {
                    word: selectedWord,
                    inputLanguage: params.inputLanguage,
                    outputLanguage: params.outputLanguage
                }, function (data) {
                    wordName = data.WordName;
                    wordFrequency = data.WordFrequencyIndex;
                    for (var i = 0; i < data.Translations.length; i++) {
                        if (savedTranslations.indexOf(data.Translations[i]) == -1) {
                            formattedData += formatTranslation(params.translationTagName, data.Translations[i]);
                        }
                    }

                    if (wordFrequency != 0) {
                        formattedData += wordFrequency.toString() + " of 12527";
                    }
                });
                
                $.ajaxSetup({ async: true });
            }

            $("#translations").html(formattedData);

            // highlights translations by adding class translationLight
            highlightWords("translation", "translationLight");

            $(params.translationTagName).click(function(e) {
                var selectedTranslation = $(this).text();
                $.post(params.addWordAction, {
                        word: wordName,
                        translation: selectedTranslation,
                        dictionaryId: currentDict
                    }, function(data) {
                        if (data) {
                            throw "Haven't added data to" + params.addWordAction;
                        } else {
                            $(params.dialogId).dialog("close");

                            $.post(params.wordListAction, { dictionaryId: currentDict }, function(data) {
                                $("#words").html(data);
                            });
                        }
                    });
            });

            // сalculates proper coordinates for the dialog 
            var x = e.pageX - $(document).scrollLeft();
            var y = e.pageY - $(document).scrollTop();

            $(params.dialogId).dialog("option", { position: [x - 100, y + 14] });

            $(params.dialogId).dialog("option", "title", wordName).dialog("open");
        });
    });
}