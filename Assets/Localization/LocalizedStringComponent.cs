using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class LocalizedStringComponent : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private LocalizedStringTable table;
    [SerializeField] private string key;

    void OnEnable() {
        table.TableChanged += LoadStrings;
    }

    void OnDisable() {
        table.TableChanged -= LoadStrings;
    }

    public void LoadStrings(StringTable stringTable) {
        text.text = GetLocalizedString(stringTable, key);
    }
    private string GetLocalizedString(StringTable table, string entryName) {
        // Get the table entry. The entry contains the localized string and Metadata
        StringTableEntry entry = table.GetEntry(entryName);
        return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here.
    }

    public void SetKey(string value) => key = value;
}
