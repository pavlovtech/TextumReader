namespace Linguistics.Models
{
    public sealed class LanguageСodeAttribute : System.Attribute
    {
        public LanguageСodeAttribute(LanguageСode lanugage)
        {
            Language = lanugage;
        }

        public LanguageСode Language { get; set; }
    }

    public enum Language
    {
        [LanguageСode(LanguageСode.ru)]
        Russian,

        [LanguageСode(LanguageСode.en)]
        English,

        [LanguageСode(LanguageСode.de)]
        German,

        [LanguageСode(LanguageСode.fr)]
        French,

        [LanguageСode(LanguageСode.it)]
        Italian,

        [LanguageСode(LanguageСode.es)]
        Spanish,

        [LanguageСode(LanguageСode.bg)]
        Bulgarian,

        [LanguageСode(LanguageСode.cs)]
        Czech,

        [LanguageСode(LanguageСode.es)]
        Estonian,

        [LanguageСode(LanguageСode.mk)]
        Macedonian,

        [LanguageСode(LanguageСode.pl)]
        Polish,

        [LanguageСode(LanguageСode.ro)]
        Romanian,

        [LanguageСode(LanguageСode.sr)]
        Serbian,

        [LanguageСode(LanguageСode.sk)]
        Slovak,

        [LanguageСode(LanguageСode.uk)]
        Ukrainian
    }
}
