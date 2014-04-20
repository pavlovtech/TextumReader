var TranslationProcessing = function(params) {
    var self = this;

    var setDialog = function() {
        $(params.dialogId).dialog({
            autoOpen: false,
            resizable: false,
            width: 210
        });
    };

    var formatTranslation = function(translationTagName, translation) {
        var formattedData =
            "<" + translationTagName + ">" +
                translation +
                "</" + translationTagName + ">" + "</br>";

        return formattedData;
    };

    self.setUp = function() {
        setDialog();

        $(params.wordTagName).click(function(e) {
            var currentDict = $("#DictionaryId option:selected").attr("value");

            var selectedWord = $(this).text();

            // TODO: replace with a parameter
            $.getJSON("/api/DictionaryAPI", {
                    word: selectedWord,
                    dictionaryId: currentDict,
                    inputLang: params.inputLanguage,
                    outputLang: params.outputLanguage
                },
                function(translationData) {
                    var formattedData = ""; // Data with translations

                    for (var i = 0; i < translationData.savedTranslations.length; i++) {
                        formattedData += "✔ " + formatTranslation(params.translationTagName, translationData.savedTranslations[i]);
                    }

                    for (var i = 0; i < translationData.translations.length; i++) {
                        formattedData += formatTranslation(params.translationTagName, translationData.translations[i]);
                    }

                    if (translationData.wordFrequency !== "") {
                        formattedData += "frequency: " + translationData.wordFrequency.toString();
                    }

                    $("#translations").html(formattedData);

                    // highlights translations by adding class translationLight
                    highlightWords("translation", "translationLight");

                    $(params.translationTagName).click(function(e) {
                        var selectedTranslation = $(this).text();

                        // TODO: replace with a parameter
                        $.post("/api/DictionaryAPI", {
                                word: translationData.word,
                                translation: selectedTranslation,
                                lang: params.inputLanguage,
                                dictionaryId: currentDict
                            }, function(data) {
                                $(params.dialogId).dialog("close");

                                $.post(params.wordListAction, { dictionaryId: currentDict }, function(data) {
                                    $("#words").html(data);
                                });
                            });
                    });

                    // TODO: take into account a word position insted of a cursor one
                    // сalculates proper coordinates for the dialog
                    var x = e.pageX - $(document).scrollLeft();
                    var y = e.pageY - $(document).scrollTop();

                    $(params.dialogId).dialog("option", { position: [x - 100, y + 14] });

                    $(params.dialogId).dialog("option", "title", translationData.word).dialog("open");
                });
        });
    };
};