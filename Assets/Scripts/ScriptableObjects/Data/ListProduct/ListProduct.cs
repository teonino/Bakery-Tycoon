using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListProduct", menuName = "Data/ListProduct")]
public class ListProduct : Data {
    [SerializeField] private List<ProductSO> defaultProduct;
    [SerializeField] private List<ProductSO> tutoProducts;
    [SerializeField] private List<ProductSO> listProduct;
    [SerializeField] private Tutorial tutorial;

    public override void ResetValues() {
        for (int i = 0; i < listProduct.Count; i++) {

            //Reset best result 
            listProduct[i].unlocked = false;

            if (tutorial.GetTutorial()) {
                for (int j = 0; j < tutoProducts.Count; j++) {
                    if (listProduct[i] == tutoProducts[j])
                        listProduct[i].unlocked = true;
                }
            }
            else {
                for (int j = 0; j < defaultProduct.Count; j++) {
                    if (listProduct[i] == defaultProduct[j])
                        listProduct[i].unlocked = true;
                }
            }
        }
    }
    public int GetProductLenght() => listProduct.Count;
    public ProductSO GetRandomProduct() => listProduct[Random.Range(0, GetProductLenght())];
    public List<ProductSO> GetProductList() => listProduct;

}
