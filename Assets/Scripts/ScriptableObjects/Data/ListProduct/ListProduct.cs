using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListProduct", menuName = "Data/ListProduct")]
public class ListProduct : ScriptableObject {
    [SerializeField] private List<ProductSO> listProduct;

    public int GetProductLenght() => listProduct.Count;
    public List<ProductSO> GetProductList() => listProduct;

}
