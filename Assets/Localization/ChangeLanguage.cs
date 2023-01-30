using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

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
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private LocalizedStringTable mainMenuTable;
    [SerializeField] private string key;

    void OnEnable()
    {
        mainMenuTable.TableChanged += LoadStrings;
    }

    void OnDisable()
    {
        mainMenuTable.TableChanged -= LoadStrings;
    }

    private void LoadStrings(StringTable stringTable)
    {
        Text.text = GetLocalizedString(stringTable, key);
    }
    static string GetLocalizedString(StringTable table, string entryName)
    {
        // Get the table entry. The entry contains the localized string and Metadata
        StringTableEntry entry = table.GetEntry(entryName);
        return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here.
    }

    private void Start()
    {
        language = (Language)GetIndex(LocalizationSettings.SelectedLocale);
    }

    private int GetIndex(Locale currentLocale)
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
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
        if (language == Language.French)
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
