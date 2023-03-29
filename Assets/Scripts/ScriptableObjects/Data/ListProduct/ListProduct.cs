using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListProduct", menuName = "Data/ListProduct")]
public class ListProduct : Data {
    [SerializeField] private List<ProductSO> defaultProduct;
    [SerializeField] private List<ProductSO> tutoProducts;
    [SerializeField] private List<ProductSO> listProduct;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private bool debug;

    public override void ResetValues() {
        for (int i = 0; i < listProduct.Count; i++) {
            listProduct[i].unlocked = false;
            listProduct[i].SetName();
        }
        if (tutorial.GetTutorial()) 
            for (int i = 0; i < tutoProducts.Count; i++)
                tutoProducts[i].unlocked = true;
        
        else if (debug) 
            for (int i = 0; i < listProduct.Count; i++)
                listProduct[i].unlocked = true;
        
        else 
            for (int i = 0; i < defaultProduct.Count; i++)
                defaultProduct[i].unlocked = true;
    }

    public int GetProductLenght() => listProduct.Count;
    public ProductSO GetRandomProduct() {
        int rng ;
        do
            rng = Random.Range(0, GetProductLenght());
        while (!listProduct[rng].unlocked || !listProduct[rng].CheckRequirement());

      return listProduct[rng];
    }
    public List<ProductSO> GetProductList() => listProduct;

}
