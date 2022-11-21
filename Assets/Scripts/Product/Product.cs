using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Product {
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

    public Product(Product product) {
        this.productSO = product.productSO;
        this.quality = product.quality;
        this.amount = product.amount;
    }

    public Product() {
        this.productSO = null;
        this.quality = 0;
        this.amount = 0;
    }
}
