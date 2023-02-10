using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CharacterTables", menuName = "Data/CharacterTables")]
public class CharacterTables : ScriptableObject
{
    [SerializeField] private LocalizedStringTable noaTable;
    [SerializeField] private LocalizedStringTable aidaTable;
    [SerializeField] private LocalizedStringTable bernardTable;
    [SerializeField] private LocalizedStringTable louletteTable;
    [SerializeField] private LocalizedStringTable yanneckTable;
    [SerializeField] private LocalizedStringTable sugarTable;
    [SerializeField] private LocalizedStringTable seraphinTable;
    [SerializeField] private LocalizedStringTable quenouilleTable;

    public LocalizedStringTable GetTable(string character) {
        LocalizedStringTable table;
        switch (character) {
            case "Noa": 
                table = noaTable; 
                break;
            case "Aida":
                table = aidaTable;
                break;
            case "Bernard":
                table = bernardTable;
                break;
            case "Loulette":
                table = louletteTable;
                break;
            case "Yanneck":
                table = yanneckTable;
                break;
            case "Sugar":
                table = sugarTable;
                break;
            case "Seraphin":
                table = seraphinTable;
                break;
            case "Quenouille":
                table = quenouilleTable;
                break;
            default:
                table = null;
                break;
        }
        return table;
    }
}
