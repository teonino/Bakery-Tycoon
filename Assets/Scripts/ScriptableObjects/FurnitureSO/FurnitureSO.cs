using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Furniture", menuName = "Furniture", order = 3)]
public class FurnitureSO : ScriptableObject {
    [SerializeField] private string furnitureName;
    [SerializeField] private int price;
    [SerializeField] private FurnitureStyle style;
    [SerializeField] private FurnitureType type;
    [SerializeField] private AssetReference assetA;
    [SerializeField] private AssetReference assetB;
    [SerializeField] private Texture imageA;
    [SerializeField] private Texture imageB;

    public string GetName() => furnitureName;
    public int GetPrice() => price;
    public FurnitureStyle GetStyle() => style;
    public new FurnitureType GetType() => type;
    public AssetReference GetAssetA() => assetA;
    public AssetReference GetAssetB() => assetB;
    public bool hasTwoAsset() => assetA.RuntimeKeyIsValid() && assetB.RuntimeKeyIsValid() && assetA.AssetGUID != assetB.AssetGUID;
    public Texture GetTextureA() => imageA;
    public Texture GetTextureB() => imageB;
}
