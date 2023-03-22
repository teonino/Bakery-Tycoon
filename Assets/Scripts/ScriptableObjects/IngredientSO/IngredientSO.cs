using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient", order = 2)]
public class IngredientSO : ScriptableObject {

    public string keyName;
    public new string name;
    public int price;
    public Texture image;
    public bool unlocked;

    public MinigameInfo minigame;

    [SerializeField] private LocalizedStringTable table;

    public void SetName() {
        StringTableEntry entry = table.GetTable().GetEntry(keyName);
        name = entry.GetLocalizedString();
    }
}
