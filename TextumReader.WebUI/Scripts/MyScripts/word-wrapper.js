function wrapAllWords(containerId, tagName) {
    var text = $(containerId).text();

    var alphabet = {};
    alphabet["russion"] = "а-я";
    alphabet["english"] = "a-z";
    alphabet["german"] = "a-zäöüß";
    alphabet["french"] = "a-û";
    alphabet["italian"] = "a-zàèéìíîòóùú";
    alphabet["spanish"] = "a-zñáéíóúü";
    alphabet["bulgarian"] = "а-я";
    alphabet["czech"] = "a-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ";
    alphabet["estonian"] = "a-zšžõäöü";
    alphabet["macedonian"] = "а-яѓјњќџ";
    alphabet["polish"] = "a-ząćęłńóóśźż";
    alphabet["romanian"] = "a-zăâîșț";
    alphabet["serbian"] = "а-яЂђјљњЋћЏџШш";
    alphabet["slovak"] = "a-záäčďdžéíĺľňóôŕšťúýž";
    alphabet["ukrainian"] = "а-я";

    var pattern = "а-яa-zäöüßûzàèéìíîòóùúzñáéíóúüÁČĎÉĚÍŇÓŘŠŤÚŮÝŽzšžõäöüѓјњќџąćęłńóóśźżăâîșțЂђјљњЋћЏџШшáäčďdžéíĺľňóôŕšťúýžґєії";

    var regexp = new RegExp("[a-zA-z" + pattern + "’'-]+", "gi");

    text = text.replace(regexp, "<" + tagName + ">$&</" + tagName + ">");
    text = text.replace(/\n/g, "<br />");
    $(containerId).html(text);
}