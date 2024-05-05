using System;
using UnityEngine;

public class NotSupportedLanguageException : Exception
{
    public SystemLanguage Language { get; private set; }
    public NotSupportedLanguageException(string message, SystemLanguage language) : base(message)
    {
        Language = language;
    }
}
public static class LanguageCodesConverter
{
    public static string ConvertToCode(SystemLanguage language)
    {
        switch (language)
        { 
            case SystemLanguage.Afrikaans: return "af";
            case SystemLanguage.Arabic: return "ar";
            case SystemLanguage.Belarusian: return "be"; 
            case SystemLanguage.Bulgarian: return "bg";
            case SystemLanguage.Basque: return "eu";
            case SystemLanguage.English: return "en"; // USA, not UK
            case SystemLanguage.Portuguese: return "pt-BR"; // Brazil, not Portugal
            case SystemLanguage.Faroese: return "fo";
            case SystemLanguage.Korean: return "ko";
            case SystemLanguage.Catalan: return "ca";
            case SystemLanguage.SerboCroatian: return "hr";
            case SystemLanguage.Czech: return "cs";
            case SystemLanguage.Danish: return "da";
            case SystemLanguage.Dutch: return "nl";
            case SystemLanguage.Estonian: return "et";
            case SystemLanguage.Finnish: return "fi";
            case SystemLanguage.French: return "fr";
            case SystemLanguage.German: return "de";
            case SystemLanguage.Greek: return "el";
            case SystemLanguage.Hebrew: return "iw";
            case SystemLanguage.Hindi: return "hi";
            case SystemLanguage.Hungarian: return "hu";
            case SystemLanguage.Icelandic: return "is";
            case SystemLanguage.Indonesian: return "id";
            case SystemLanguage.Italian: return "it";
            case SystemLanguage.Japanese: return "ja";
            case SystemLanguage.Latvian: return "lv";
            case SystemLanguage.Lithuanian: return "lt";
            case SystemLanguage.Norwegian: return "no";
            case SystemLanguage.Polish: return "pl";
            case SystemLanguage.Romanian: return "ro";
            case SystemLanguage.Russian: return "ru";
            case SystemLanguage.ChineseSimplified: return "zh-CN";
            case SystemLanguage.ChineseTraditional: return "zh-TW";
            case SystemLanguage.Slovak: return "sk";
            case SystemLanguage.Slovenian: return "sl";
            case SystemLanguage.Spanish: return "es";
            case SystemLanguage.Swedish: return "sv";
            case SystemLanguage.Thai: return "th";
            case SystemLanguage.Turkish: return "tr";
            case SystemLanguage.Ukrainian: return "uk";
            case SystemLanguage.Vietnamese: return "vi";
            default: throw new NotSupportedLanguageException($"{language} is not supported by Google Translate", language);
        }
    }
    /*
hl=af          Afrikaans
hl=ak          Akan
hl=sq          Albanian
hl=am          Amharic
hl=ar          Arabic
hl=hy          Armenian
hl=az          Azerbaijani
hl=eu          Basque
hl=be          Belarusian
hl=bem         Bemba
hl=bn          Bengali
hl=bh          Bihari
hl=xx-bork     Bork, bork, bork!
hl=bs          Bosnian
hl=br          Breton
hl=bg          Bulgarian
hl=km          Cambodian
hl=ca          Catalan
hl=chr         Cherokee
hl=ny          Chichewa
hl=zh-CN       Chinese (Simplified)
hl=zh-TW       Chinese (Traditional)
hl=co          Corsican
hl=hr          Croatian
hl=cs          Czech
hl=da          Danish
hl=nl          Dutch
hl=xx-elmer    Elmer Fudd
hl=en          English
hl=eo          Esperanto
hl=et          Estonian
hl=ee          Ewe
hl=fo          Faroese
hl=tl          Filipino
hl=fi          Finnish
hl=fr          French
hl=fy          Frisian
hl=gaa         Ga
hl=gl          Galician
hl=ka          Georgian
hl=de          German
hl=el          Greek
hl=gn          Guarani
hl=gu          Gujarati
hl=xx-hacker   Hacker
hl=ht          Haitian Creole
hl=ha          Hausa
hl=haw         Hawaiian
hl=iw          Hebrew
hl=hi          Hindi
hl=hu          Hungarian
hl=is          Icelandic
hl=ig          Igbo
hl=id          Indonesian
hl=ia          Interlingua
hl=ga          Irish
hl=it          Italian
hl=ja          Japanese
hl=jw          Javanese
hl=kn          Kannada
hl=kk          Kazakh
hl=rw          Kinyarwanda
hl=rn          Kirundi
hl=xx-klingon  Klingon
hl=kg          Kongo
hl=ko          Korean
hl=kri         Krio (Sierra Leone)
hl=ku          Kurdish
hl=ckb         Kurdish (Soranî)
hl=ky          Kyrgyz
hl=lo          Laothian
hl=la          Latin
hl=lv          Latvian
hl=ln          Lingala
hl=lt          Lithuanian
hl=loz         Lozi
hl=lg          Luganda
hl=ach         Luo
hl=mk          Macedonian
hl=mg          Malagasy
hl=ms          Malay
hl=ml          Malayalam
hl=mt          Maltese
hl=mi          Maori
hl=mr          Marathi
hl=mfe         Mauritian Creole
hl=mo          Moldavian
hl=mn          Mongolian
hl=sr-ME       Montenegrin
hl=ne          Nepali
hl=pcm         Nigerian Pidgin
hl=nso         Northern Sotho
hl=no          Norwegian
hl=nn          Norwegian (Nynorsk)
hl=oc          Occitan
hl=or          Oriya
hl=om          Oromo
hl=ps          Pashto
hl=fa          Persian
hl=xx-pirate   Pirate
hl=pl          Polish
hl=pt-BR       Portuguese (Brazil)
hl=pt-PT       Portuguese (Portugal)
hl=pa          Punjabi
hl=qu          Quechua
hl=ro          Romanian
hl=rm          Romansh
hl=nyn         Runyakitara
hl=ru          Russian
hl=gd          Scots Gaelic
hl=sr          Serbian
hl=sh          Serbo-Croatian
hl=st          Sesotho
hl=tn          Setswana
hl=crs         Seychellois Creole
hl=sn          Shona
hl=sd          Sindhi
hl=si          Sinhalese
hl=sk          Slovak
hl=sl          Slovenian
hl=so          Somali
hl=es          Spanish
hl=es-419      Spanish (Latin American)
hl=su          Sundanese
hl=sw          Swahili
hl=sv          Swedish
hl=tg          Tajik
hl=ta          Tamil
hl=tt          Tatar
hl=te          Telugu
hl=th          Thai
hl=ti          Tigrinya
hl=to          Tonga
hl=lua         Tshiluba
hl=tum         Tumbuka
hl=tr          Turkish
hl=tk          Turkmen
hl=tw          Twi
hl=ug          Uighur
hl=uk          Ukrainian
hl=ur          Urdu
hl=uz          Uzbek
hl=vi          Vietnamese
hl=cy          Welsh
hl=wo          Wolof
hl=xh          Xhosa
hl=yi          Yiddish
hl=yo          Yoruba
hl=zu          Zulu
*/
}
