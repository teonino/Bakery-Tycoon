using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductSO product;
    public int amount;

    public string GetName() => product.name;
    public float GetPrice() => product.price;


}
