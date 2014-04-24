var PopoverManager = function(params) {
    var self = this;

    var translatioObject;

    var formatTranslation = function(translationTagName, translation) {
        var formattedData =
            "<" + translationTagName + ">" +
                translation +
                "</" + translationTagName + ">" + "</br>";

        return formattedData;
    };

    self.getSelected = function() {
        var t = '';
        if (window.getSelection) {
            t = window.getSelection();
        } else if (document.getSelection) {
            t = document.getSelection();
        } else if (document.selection) {
            t = document.selection.createRange().text;
        }
        return t;
    };

    self.setUp = function () {
        $(document).bind("mouseup", function(e) {
            var currentDict = $("#DictionaryId option:selected").attr("value");
            
            var st = self.getSelected();
            if (st != '') {
                var sentencePopover = $("#sentenceTranslationPopover");
                sentencePopover.popover('destroy');

                $.getJSON("/api/DictionaryAPI/GetSentenceTranslation", {
                    sentence: st.toString(),
                    inputLang: params.inputLanguage,
                    outputLang: params.outputLanguage
                }, function (data) {
                    sentencePopover.popover({
                        content: function() {
                            return data;
                        },
                    }).popover('show');
                    
                    var left = e.pageX;
                    var top = e.pageY;
                    var theHeight = $('.popover').height();
                    $('.popover').css('left', (left + 10) + 'px');
                    $('.popover').css('top', (top - (theHeight / 2) - 10) + 'px');
                });
            }
        });

        $(params.wordTagName).click(function(e) {
            var wordElement = $(this);

            var currentDict = $("#DictionaryId option:selected").attr("value");

            var selectedWord = wordElement.text();

            // TODO: replace with a parameter
            $.getJSON("/api/DictionaryAPI/GetTranslations", {
                    word: selectedWord,
                    dictionaryId: currentDict,
                    inputLang: params.inputLanguage,
                    outputLang: params.outputLanguage
                },
                function (translationData) {
                    translatioObject = translationData;

                    var formattedData = "";

                    for (var i = 0; i < translationData.savedTranslations.length; i++) {
                        formattedData += "✔ " + formatTranslation(params.translationTagName, translationData.savedTranslations[i]);
                    }

                    for (var i = 0; i < translationData.translations.length; i++) {
                        formattedData += formatTranslation(params.translationTagName, translationData.translations[i]);
                    }

                    if (translationData.wordFrequency !== "") {
                        formattedData += "frequency: " + translationData.wordFrequency.toString();
                    }

                    $(params.popoverId).html(formattedData);

                    wordElement.popover({
                        content: function() {
                            return $(params.popoverId).html();
                        },
                        title: "<a class='glyphicon glyphicon-play-circle' style='text-decoration:none; padding-right:5px; cursor: pointer' id='audio'></a> " + translationData.word,
                        html: true,
                        trigger: 'manual',
                        placement: 'bottom',
                        container: 'body'
                    }).popover('show');

                    $("#audio").click(function () {
                        var audio = new Audio(translationData.audioUrl);
                        audio.load();
                        audio.play();
                    });

                    // highlights translations by adding class translationLight
                    highlightWords("translation", "translationLight");

                    $(params.translationTagName).click(function() {
                        var selectedTranslation = $(this).text();
                        // TODO: replace with a parameter
                        $.post("/api/DictionaryAPI/PostWord", {
                                word: translationData.word,
                                translation: selectedTranslation,
                                lang: params.inputLanguage,
                                dictionaryId: currentDict
                            }, function() {
                                $.post(params.wordListAction, { dictionaryId: currentDict }, function (data) {
                                    wordElement.popover('destroy');
                                    $("#words").html(data);
                                });
                            });
                    });
                });
        });
    };
}