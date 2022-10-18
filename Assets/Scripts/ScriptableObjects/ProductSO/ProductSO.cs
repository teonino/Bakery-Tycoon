using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Product", menuName = "Product", order = 1)]
public class ProductSO : ScriptableObject {
    [Header("Global variables")]
    public new string name;
    public float price;
    public int nbCreated;
    public float recipePrice;
    public bool unlocked;
    public AssetReference asset;
    public Texture image;

    [Space(10)]
    [Header("Requirement")]
    public bool hoven;
    public List<IngredientSO> ingredients;

    [Space(10)]
    [Header("Minigames")]
    //public List<Minigame> minigames;
    public List<MinigameInfo> minigames;
}
