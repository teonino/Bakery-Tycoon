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
        StringTableEntry entry = table.GetEntry(entryName);
        if (entry != null)
            if (entry.GetLocalizedString().Contains('+'))
                return entry.GetLocalizedString().Replace('+', ' ');
            else
                return entry.GetLocalizedString();
        else
            return string.Empty;
    }

    public void SetKey(string value) => key = value;
    public void SetTable(LocalizedStringTable table) => this.table = table;
}
