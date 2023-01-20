using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Product", menuName = "Product", order = 1)]
public class ProductSO : ScriptableObject {
    [Header("Global variables")]
    public new string name;
    public int price;
    public float recipePrice;
    public int nbCreated;
    public float cookingTime;
    public bool unlocked = false;
    public Texture image;
    public AssetReference asset;

    [Space(5)]
    [Header("Paste")]
    public AssetReference pasteAsset = null;

    [Space(5)]
    [Header("Requirement")]
    public CraftingStationType craftStationRequired;
    public List<IngredientsForProduct> ingredients;


    [Space(5)]
    [Header("Minigames")]
    public List<MinigameInfo> minigames;

    public bool CheckRequirement() {
        bool requirementMet = true;

        //Check Crafting Station
        List<CraftingStation> craftingStations = new List<CraftingStation>(FindObjectsOfType<CraftingStation>());
        if (requirementMet) {
            requirementMet = false;
            foreach (CraftingStation craftingStation in craftingStations)
                if (craftingStation.GetCraftingStationType() == craftStationRequired) 
                    requirementMet = true;
        }

        return requirementMet;
    }
}
