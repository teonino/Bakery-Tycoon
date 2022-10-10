using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Product", menuName = "Product", order = 1)]
public class ProductSO : ScriptableObject
{
    public string name;
    public float price;
    public AssetReference asset;
}
