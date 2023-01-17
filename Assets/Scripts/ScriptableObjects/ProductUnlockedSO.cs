using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductUnlocked", menuName = "Action/ProductUnlocked")]
public class ProductUnlockedSO : ScriptableObject
{
    public Action<ProductSO> action;

    public void Invoke(ProductSO product) => action?.Invoke(product);
}
