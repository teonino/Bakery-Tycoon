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
            productModels[product.GetAmount() - 1].gameObject.SetActive(false);
    }
}
