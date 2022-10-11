using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Product", menuName = "Product", order = 1)]
public class ProductSO : ScriptableObject {
    [Header("Global variables")]
    public new string name;
    public float price;
    public AssetReference asset;
    public Texture image;

    [Space(10)]
    [Header("Requirement")]
    public bool hoven;

    [Space(10)]
    [Header("Minigames")]
    public List<Minigame> minigames;

    public bool CheckRequirement() {
        bool requirementMet = true;
        List<CraftingStation> craftingStations = new List<CraftingStation>(FindObjectsOfType<CraftingStation>());

        if (hoven && requirementMet) { // If hoven is a requirement and requirementMet is still true
            requirementMet = false;
            foreach (CraftingStation craftingStation in craftingStations) {
                if (craftingStation.type == CraftingStationType.Hoven) {
                    requirementMet = true;
                }
            }
        }

        return requirementMet;
    }
}
