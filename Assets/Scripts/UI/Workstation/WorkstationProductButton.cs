using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class WorkstationProductButton : MonoBehaviour {

    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI productDescription;
    [SerializeField] private AssetReference ingredientAsset;
    [SerializeField] private GameObject layoutGroup;

    private ProductSO product;
    private Texture defaultImage;

    private void Awake() {
        defaultImage = GetComponentInChildren<RawImage>().texture;
    }

    public void SetProduct(ProductSO product) {
        this.product = product;
        DislayProduct();
    }

    public void DislayProduct() {
        if (product.unlocked) {
            image.texture = product.image;

            productName.SetText(product.name);
            productDescription.SetText("Ingredients :\n");

        }
        else {
            image.texture = defaultImage;

            productName.SetText("??????????");
            productDescription.SetText("??????????");

        }
        foreach(Transform item in layoutGroup.transform)
            item.gameObject.SetActive(false);

        for (int i = 0; i < product.ingredients.Count; i++) {
            layoutGroup.transform.GetChild(i).gameObject.SetActive(true);
            if (product.unlocked || product.ingredients[i].isUnlocked())
                layoutGroup.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = product.ingredients[i].ingredient.name;
            else
                layoutGroup.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "??????????";

        }
    }

    public ProductSO GetProduct() => this.product;
}
