function wrapAllWords(containerId, tagName) {
    var text = $(containerId).text();

    var lphabet = {};
    lphabet["russion"] = "а-я";
    lphabet["english"] = "a-z";
    lphabet["german"] = "a-zäöüß";
    lphabet["french"] = "a-û";
    lphabet["italian"] = "a-zàèéìíîòóùú";
    lphabet["spanish"] = "a-zñáéíóúü";
    lphabet["bulgarian"] = "а-я";
    lphabet["czech"] = "a-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ";
    lphabet["estonian"] = "a-zšžõäöü";
    lphabet["macedonian"] = "а-яѓјњќџ";
    lphabet["polish"] = "a-ząćęłńóóśźż";
    lphabet["romanian"] = "a-zăâîșț";
    lphabet["serbian"] = "а-яЂђјљњЋћЏџШш";
    lphabet["slovak"] = "a-záäčďdžéíĺľňóôŕšťúýž";
    lphabet["ukrainian"] = "а-я";

    var pattern = "а-яa-zäöüßûzàèéìíîòóùúzñáéíóúüÁČĎÉĚÍŇÓŘŠŤÚŮÝŽzšžõäöüѓјњќџąćęłńóóśźżăâîșțЂђјљњЋћЏџШшáäčďdžéíĺľňóôŕšťúýžґєії";

    var regexp = new RegExp("[a-zA-z" + pattern + "’'-]+", "gi");

    text = text.replace(regexp, "<" + tagName + ">$&</" + tagName + ">");
    text = text.replace(/\n/g, "<br />");
    $(containerId).html(text);
}