using System;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    #region Exceptions
    public class NotSupportedSystemLanguageException : Exception
    {
        public SystemLanguage Language { get; private set; }
        public NotSupportedSystemLanguageException(string message, SystemLanguage language) : base(message)
        {
            Language = language;
        }
    }
    public class NotSupportedApplicationLanguageException : Exception
    {
        public ApplicationLanguage Language { get; private set; }
        public NotSupportedApplicationLanguageException(string message, ApplicationLanguage language) : base(message)
        {
            Language = language;
        }
    }
    #endregion
    public static class LanguageCodesConverter
    {
        #region IsGoogleTranslateException
        public static bool IsGoogleTranslateException(ApplicationLanguage systemLanguage) // Google returns an error when sending these languages
        {
            switch(systemLanguage)
            {
                case ApplicationLanguage.Bemba: return true;
                case ApplicationLanguage.Bork: return true;
                case ApplicationLanguage.Breton: return true;
                case ApplicationLanguage.Cherokee: return true;
                case ApplicationLanguage.Faroese: return true;
                case ApplicationLanguage.Hacker: return true;
                case ApplicationLanguage.Interlingua: return true;
                case ApplicationLanguage.Kirundi: return true;
                case ApplicationLanguage.Klingon: return true;
                case ApplicationLanguage.Kongo: return true;
                case ApplicationLanguage.Lozi: return true;
                case ApplicationLanguage.Luo: return true;
                case ApplicationLanguage.Mauritian_Creole: return true;
                case ApplicationLanguage.Nigerian_Pidgin: return true;
                case ApplicationLanguage.Norwegian_Nynorsk: return true;
                case ApplicationLanguage.Occitan: return true;
                case ApplicationLanguage.Pirate: return true;
                case ApplicationLanguage.Romansh: return true;
                case ApplicationLanguage.Runyakitara: return true;
                case ApplicationLanguage.Scots: return true;
                case ApplicationLanguage.Setswana: return true;
                case ApplicationLanguage.Seychellois_Creole: return true;
                case ApplicationLanguage.Tonga: return true;
                case ApplicationLanguage.Tshiluba: return true;
                case ApplicationLanguage.Tumbuka: return true;
                case ApplicationLanguage.Wolof: return true;
                default: return false;
            }
        }
        #endregion
        #region ConvertSystemLanguageToCode
        public static string ConvertSystemLanguageToCode(SystemLanguage systemLanguage)
        {
            switch (systemLanguage)
            {
                case SystemLanguage.Afrikaans:
                    return "af";
                case SystemLanguage.Arabic:
                    return "ar";
                case SystemLanguage.Basque:
                    return "eu";
                case SystemLanguage.Belarusian:
                    return "be";
                case SystemLanguage.Bulgarian:
                    return "bg";
                case SystemLanguage.Catalan:
                    return "ca";
                case SystemLanguage.Chinese:
                    return "zh"; // For Chinese, it's better to use the generic "zh" code, as there are variations.
                case SystemLanguage.Czech:
                    return "cs";
                case SystemLanguage.Danish:
                    return "da";
                case SystemLanguage.Dutch:
                    return "nl";
                case SystemLanguage.English:
                    return "en";
                case SystemLanguage.Estonian:
                    return "et";
                case SystemLanguage.Faroese:
                    return "fo";
                case SystemLanguage.Finnish:
                    return "fi";
                case SystemLanguage.French:
                    return "fr";
                case SystemLanguage.German:
                    return "de";
                case SystemLanguage.Greek:
                    return "el";
                case SystemLanguage.Hebrew:
                    return "he";
                case SystemLanguage.Hungarian:
                    return "hu";
                case SystemLanguage.Icelandic:
                    return "is";
                case SystemLanguage.Indonesian:
                    return "id";
                case SystemLanguage.Italian:
                    return "it";
                case SystemLanguage.Japanese:
                    return "ja";
                case SystemLanguage.Korean:
                    return "ko";
                case SystemLanguage.Latvian:
                    return "lv";
                case SystemLanguage.Lithuanian:
                    return "lt";
                case SystemLanguage.Norwegian:
                    return "no";
                case SystemLanguage.Polish:
                    return "pl";
                case SystemLanguage.Portuguese:
                    return "pt";
                case SystemLanguage.Romanian:
                    return "ro";
                case SystemLanguage.Russian:
                    return "ru";
                case SystemLanguage.SerboCroatian:
                    return "sh";
                case SystemLanguage.Slovak:
                    return "sk";
                case SystemLanguage.Slovenian:
                    return "sl";
                case SystemLanguage.Spanish:
                    return "es";
                case SystemLanguage.Swedish:
                    return "sv";
                case SystemLanguage.Thai:
                    return "th";
                case SystemLanguage.Turkish:
                    return "tr";
                case SystemLanguage.Ukrainian:
                    return "uk";
                case SystemLanguage.Vietnamese:
                    return "vi";
                case SystemLanguage.ChineseSimplified:
                    return "zh-CN";
                case SystemLanguage.ChineseTraditional:
                    return "zh-TW";
                default: throw new NotSupportedSystemLanguageException($"{systemLanguage} is not supported by Google Translate", systemLanguage);
            }
        }
        #endregion
        #region ConvertToApplicationLanguage
        public static ApplicationLanguage ConvertToApplicationLanguage(SystemLanguage systemLanguage)
        {
            switch (systemLanguage)
            {
                case SystemLanguage.Afrikaans:
                    return ApplicationLanguage.Afrikaans;
                case SystemLanguage.Arabic:
                    return ApplicationLanguage.Arabic;
                case SystemLanguage.Basque:
                    return ApplicationLanguage.Basque;
                case SystemLanguage.Belarusian:
                    return ApplicationLanguage.Belarusian;
                case SystemLanguage.Bulgarian:
                    return ApplicationLanguage.Bulgarian;
                case SystemLanguage.Catalan:
                    return ApplicationLanguage.Catalan;
                case SystemLanguage.Chinese:
                    return ApplicationLanguage.Chinese_Traditional; // Could be either Simplified or Traditional. Defaulting to Traditional.
                case SystemLanguage.Czech:
                    return ApplicationLanguage.Czech;
                case SystemLanguage.Danish:
                    return ApplicationLanguage.Danish;
                case SystemLanguage.Dutch:
                    return ApplicationLanguage.Dutch;
                case SystemLanguage.English:
                    return ApplicationLanguage.English;
                case SystemLanguage.Estonian:
                    return ApplicationLanguage.Estonian;
                case SystemLanguage.Faroese:
                    return ApplicationLanguage.Faroese;
                case SystemLanguage.Finnish:
                    return ApplicationLanguage.Finnish;
                case SystemLanguage.French:
                    return ApplicationLanguage.French;
                case SystemLanguage.German:
                    return ApplicationLanguage.German;
                case SystemLanguage.Greek:
                    return ApplicationLanguage.Greek;
                case SystemLanguage.Hebrew:
                    return ApplicationLanguage.Hebrew;
                case SystemLanguage.Hungarian:
                    return ApplicationLanguage.Hungarian;
                case SystemLanguage.Icelandic:
                    return ApplicationLanguage.Icelandic;
                case SystemLanguage.Indonesian:
                    return ApplicationLanguage.Indonesian;
                case SystemLanguage.Italian:
                    return ApplicationLanguage.Italian;
                case SystemLanguage.Japanese:
                    return ApplicationLanguage.Japanese;
                case SystemLanguage.Korean:
                    return ApplicationLanguage.Korean;
                case SystemLanguage.Latvian:
                    return ApplicationLanguage.Latvian;
                case SystemLanguage.Lithuanian:
                    return ApplicationLanguage.Lithuanian;
                case SystemLanguage.Norwegian:
                    return ApplicationLanguage.Norwegian;
                case SystemLanguage.Polish:
                    return ApplicationLanguage.Polish;
                case SystemLanguage.Portuguese:
                    return ApplicationLanguage.Portuguese_Portugal; // Could be either Portugal or Brazil. Defaulting to Portugal.
                case SystemLanguage.Romanian:
                    return ApplicationLanguage.Romanian;
                case SystemLanguage.Russian:
                    return ApplicationLanguage.Russian;
                case SystemLanguage.SerboCroatian:
                    return ApplicationLanguage.Serbo_Croatian;
                case SystemLanguage.Slovak:
                    return ApplicationLanguage.Slovak;
                case SystemLanguage.Slovenian:
                    return ApplicationLanguage.Slovenian;
                case SystemLanguage.Spanish:
                    return ApplicationLanguage.Spanish;
                case SystemLanguage.Swedish:
                    return ApplicationLanguage.Swedish;
                case SystemLanguage.Thai:
                    return ApplicationLanguage.Thai;
                case SystemLanguage.Turkish:
                    return ApplicationLanguage.Turkish;
                case SystemLanguage.Ukrainian:
                    return ApplicationLanguage.Ukrainian;
                case SystemLanguage.Vietnamese:
                    return ApplicationLanguage.Vietnamese;
                case SystemLanguage.ChineseSimplified:
                    return ApplicationLanguage.Chinese_Simplified;
                case SystemLanguage.ChineseTraditional:
                    return ApplicationLanguage.Chinese_Traditional;
                default:
                    throw new NotSupportedSystemLanguageException($"{systemLanguage} is not supported", systemLanguage);
            }
        }
        #endregion
        #region ConvertApplicationLanguageToHLCode
        public static string ConvertApplicationLanguageToHLCode(ApplicationLanguage language)
        {
            switch (language)
            {
                case ApplicationLanguage.Afrikaans:
                    return "af";
                case ApplicationLanguage.Akan:
                    return "ak";
                case ApplicationLanguage.Albanian:
                    return "sq";
                case ApplicationLanguage.Amharic:
                    return "am";
                case ApplicationLanguage.Arabic:
                    return "ar";
                case ApplicationLanguage.Armenian:
                    return "hy";
                case ApplicationLanguage.Azerbaijani:
                    return "az";
                case ApplicationLanguage.Basque:
                    return "eu";
                case ApplicationLanguage.Belarusian:
                    return "be";
                case ApplicationLanguage.Bemba:
                    return "bem";
                case ApplicationLanguage.Bengali:
                    return "bn";
                case ApplicationLanguage.Bihari:
                    return "bh";
                case ApplicationLanguage.Bork:
                    return "xx-bork";
                case ApplicationLanguage.Bosnian:
                    return "bs";
                case ApplicationLanguage.Breton:
                    return "br";
                case ApplicationLanguage.Bulgarian:
                    return "bg";
                case ApplicationLanguage.Cambodian:
                    return "km";
                case ApplicationLanguage.Catalan:
                    return "ca";
                case ApplicationLanguage.Cherokee:
                    return "chr";
                case ApplicationLanguage.Chichewa:
                    return "ny";
                case ApplicationLanguage.Chinese_Simplified:
                    return "zh-cn";
                case ApplicationLanguage.Chinese_Traditional:
                    return "zh-tw";
                case ApplicationLanguage.Corsican:
                    return "co";
                case ApplicationLanguage.Croatian:
                    return "hr";
                case ApplicationLanguage.Czech:
                    return "cs";
                case ApplicationLanguage.Danish:
                    return "da";
                case ApplicationLanguage.Dutch:
                    return "nl";
                case ApplicationLanguage.Elmer:
                    return "elmer";
                case ApplicationLanguage.English:
                    return "en";
                case ApplicationLanguage.Esperanto:
                    return "eo";
                case ApplicationLanguage.Estonian:
                    return "et";
                case ApplicationLanguage.Ewe:
                    return "ee";
                case ApplicationLanguage.Faroese:
                    return "fo";
                case ApplicationLanguage.Filipino:
                    return "fil";
                case ApplicationLanguage.Finnish:
                    return "fi";
                case ApplicationLanguage.French:
                    return "fr";
                case ApplicationLanguage.Frisian:
                    return "fy";
                case ApplicationLanguage.Ga:
                    return "ga";
                case ApplicationLanguage.Galician:
                    return "gl";
                case ApplicationLanguage.Georgian:
                    return "ka";
                case ApplicationLanguage.German:
                    return "de";
                case ApplicationLanguage.Greek:
                    return "el";
                case ApplicationLanguage.Guarani:
                    return "gn";
                case ApplicationLanguage.Gujarati:
                    return "gu";
                case ApplicationLanguage.Hacker:
                    return "xx-hacker";
                case ApplicationLanguage.Haitian:
                    return "ht";
                case ApplicationLanguage.Hausa:
                    return "ha";
                case ApplicationLanguage.Hawaiian:
                    return "haw";
                case ApplicationLanguage.Hebrew:
                    return "he";
                case ApplicationLanguage.Hindi:
                    return "hi";
                case ApplicationLanguage.Hungarian:
                    return "hu";
                case ApplicationLanguage.Icelandic:
                    return "is";
                case ApplicationLanguage.Igbo:
                    return "ig";
                case ApplicationLanguage.Indonesian:
                    return "id";
                case ApplicationLanguage.Interlingua:
                    return "ia";
                case ApplicationLanguage.Irish:
                    return "ga";
                case ApplicationLanguage.Italian:
                    return "it";
                case ApplicationLanguage.Japanese:
                    return "ja";
                case ApplicationLanguage.Javanese:
                    return "jv";
                case ApplicationLanguage.Kannada:
                    return "kn";
                case ApplicationLanguage.Kazakh:
                    return "kk";
                case ApplicationLanguage.Kinyarwanda:
                    return "rw";
                case ApplicationLanguage.Kirundi:
                    return "rn";
                case ApplicationLanguage.Klingon:
                    return "xx-klingon";
                case ApplicationLanguage.Kongo:
                    return "kg";
                case ApplicationLanguage.Korean:
                    return "ko";
                case ApplicationLanguage.Krio:
                    return "kri";
                case ApplicationLanguage.Kurdish:
                    return "ku";
                case ApplicationLanguage.Kurdish_Soranî:
                    return "ckb";
                case ApplicationLanguage.Kyrgyz:
                    return "ky";
                case ApplicationLanguage.Laothian:
                    return "lo";
                case ApplicationLanguage.Latin:
                    return "la";
                case ApplicationLanguage.Latvian:
                    return "lv";
                case ApplicationLanguage.Lingala:
                    return "ln";
                case ApplicationLanguage.Lithuanian:
                    return "lt";
                case ApplicationLanguage.Lozi:
                    return "loz";
                case ApplicationLanguage.Luganda:
                    return "lg";
                case ApplicationLanguage.Luo:
                    return "ach";
                case ApplicationLanguage.Macedonian:
                    return "mk";
                case ApplicationLanguage.Malagasy:
                    return "mg";
                case ApplicationLanguage.Malay:
                    return "ms";
                case ApplicationLanguage.Malayalam:
                    return "ml";
                case ApplicationLanguage.Maltese:
                    return "mt";
                case ApplicationLanguage.Maori:
                    return "mi";
                case ApplicationLanguage.Marathi:
                    return "mr";
                case ApplicationLanguage.Mauritian_Creole:
                    return "mfe";
                case ApplicationLanguage.Moldavian:
                    return "mo";
                case ApplicationLanguage.Mongolian:
                    return "mn";
                case ApplicationLanguage.Montenegrin:
                    return "sr-me";
                case ApplicationLanguage.Nepali:
                    return "ne";
                case ApplicationLanguage.Nigerian_Pidgin:
                    return "pcm";
                case ApplicationLanguage.Northern_Sotho:
                    return "nso";
                case ApplicationLanguage.Norwegian:
                    return "no";
                case ApplicationLanguage.Norwegian_Nynorsk:
                    return "nn";
                case ApplicationLanguage.Occitan:
                    return "oc";
                case ApplicationLanguage.Oriya:
                    return "or";
                case ApplicationLanguage.Oromo:
                    return "om";
                case ApplicationLanguage.Pashto:
                    return "ps";
                case ApplicationLanguage.Persian:
                    return "fa";
                case ApplicationLanguage.Pirate:
                    return "xx-pirate";
                case ApplicationLanguage.Polish:
                    return "pl";
                case ApplicationLanguage.Portuguese_Brazil:
                    return "pt-br";
                case ApplicationLanguage.Portuguese_Portugal:
                    return "pt-pt";
                case ApplicationLanguage.Punjabi:
                    return "pa";
                case ApplicationLanguage.Quechua:
                    return "qu";
                case ApplicationLanguage.Romanian:
                    return "ro";
                case ApplicationLanguage.Romansh:
                    return "rm";
                case ApplicationLanguage.Runyakitara:
                    return "rn";
                case ApplicationLanguage.Russian:
                    return "ru";
                case ApplicationLanguage.Scots:
                    return "sco";
                case ApplicationLanguage.Serbian:
                    return "sr";
                case ApplicationLanguage.Serbo_Croatian:
                    return "sh";
                case ApplicationLanguage.Sesotho:
                    return "st";
                case ApplicationLanguage.Setswana:
                    return "tn";
                case ApplicationLanguage.Seychellois_Creole:
                    return "crs";
                case ApplicationLanguage.Shona:
                    return "sn";
                case ApplicationLanguage.Sindhi:
                    return "sd";
                case ApplicationLanguage.Sinhalese:
                    return "si";
                case ApplicationLanguage.Slovak:
                    return "sk";
                case ApplicationLanguage.Slovenian:
                    return "sl";
                case ApplicationLanguage.Somali:
                    return "so";
                case ApplicationLanguage.Spanish:
                    return "es";
                case ApplicationLanguage.Spanish_LatinAmerican:
                    return "es-419";
                case ApplicationLanguage.Sundanese:
                    return "su";
                case ApplicationLanguage.Swahili:
                    return "sw";
                case ApplicationLanguage.Swedish:
                    return "sv";
                case ApplicationLanguage.Tajik:
                    return "tg";
                case ApplicationLanguage.Tamil:
                    return "ta";
                case ApplicationLanguage.Tatar:
                    return "tt";
                case ApplicationLanguage.Telugu:
                    return "te";
                case ApplicationLanguage.Thai:
                    return "th";
                case ApplicationLanguage.Tigrinya:
                    return "ti";
                case ApplicationLanguage.Tonga:
                    return "to";
                case ApplicationLanguage.Tshiluba:
                    return "lua";
                case ApplicationLanguage.Tumbuka:
                    return "tum";
                case ApplicationLanguage.Turkish:
                    return "tr";
                case ApplicationLanguage.Turkmen:
                    return "tk";
                case ApplicationLanguage.Twi:
                    return "tw";
                case ApplicationLanguage.Uighur:
                    return "ug";
                case ApplicationLanguage.Ukrainian:
                    return "uk";
                case ApplicationLanguage.Urdu:
                    return "ur";
                case ApplicationLanguage.Uzbek:
                    return "uz";
                case ApplicationLanguage.Vietnamese:
                    return "vi";
                case ApplicationLanguage.Welsh:
                    return "cy";
                case ApplicationLanguage.Wolof:
                    return "wo";
                case ApplicationLanguage.Xhosa:
                    return "xh";
                case ApplicationLanguage.Yiddish:
                    return "yi";
                case ApplicationLanguage.Yoruba:
                    return "yo";
                case ApplicationLanguage.Zulu:
                    return "zu";
                default: throw new NotSupportedApplicationLanguageException($"{language} is not supported", language);
            }
        }
        #endregion
    }
}

