using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Furniture", menuName = "Furniture", order = 3)]
public class FurnitureSO : ScriptableObject {
    [SerializeField] private new string name;
    [SerializeField] private int price;
    [SerializeField] private FurnitureStyle style;
    [SerializeField] private FurnitureType type;
    [SerializeField] private AssetReference asset;
    [SerializeField] private Texture image;

    public string GetName() => name;
    public int GetPrice() => price;
    public FurnitureStyle GetStyle() => style;
    public new FurnitureType GetType() => type;
    public AssetReference GetAssets() => asset;
    public Texture GetTexture() => image;
}
