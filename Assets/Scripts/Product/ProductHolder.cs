using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ProductHolder : MonoBehaviour {
    public Product product;
    public List<GameObject> productModels;
    public bool blocked = false;

    private void OnDestroy() {
        Addressables.ReleaseInstance(gameObject);
    }

    public void RemoveAmount() {
        product.RemoveAmount();
        if (productModels != null)
            productModels[product.GetAmount()].gameObject.SetActive(false);
    }

    public void SetAmount(int i) {
        product.SetAmount(i);

        for(int j = productModels.Count - 1; j > i; j--) {
            productModels[j].gameObject.SetActive(false);
        }
    }

    public void DisplayOneProduct() {
        foreach (GameObject model in productModels) {
            model.SetActive(false);
        }
        productModels[0].SetActive(true);
    }
}
