using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductSO productSO;
    public int quality;
    public int amount;

    public string GetName() => productSO.name;
    public float GetPrice() => productSO.price;
    public CraftingStationType GetCraftingStation() => productSO.craftStationRequired;
}
