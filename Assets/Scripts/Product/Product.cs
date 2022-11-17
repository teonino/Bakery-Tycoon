using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductSO productSO;
    public int quality;
    public int amount;

    public void SetProduct(ProductSO product) {
        productSO = product;
        amount = product.nbCreated;
    }
    public string GetName() => productSO.name;
    public float GetPrice() => productSO.price;
    public CraftingStationType GetCraftingStation() => productSO.craftStationRequired;
}
