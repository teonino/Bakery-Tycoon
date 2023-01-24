using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public enum Language
{
    English,
    French,
    German,
    Italian
    //Spain,
}

public class ChangeLanguage : MonoBehaviour
{
     Language language;
    
    private void Start()
    {
        language = (Language) GetIndex(LocalizationSettings.SelectedLocale);
    }
    
    private int GetIndex(Locale currentLocale)
    {
        for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (currentLocale == LocalizationSettings.AvailableLocales.Locales[i])
            {
                return i;
            }
        }
        return 0;
    }

    public void ChangeLanguageButton()
    {
        if(language == Language.French)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            language = (Language)GetIndex(LocalizationSettings.SelectedLocale);
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            language = (Language)GetIndex(LocalizationSettings.SelectedLocale);
        }
    }
}
