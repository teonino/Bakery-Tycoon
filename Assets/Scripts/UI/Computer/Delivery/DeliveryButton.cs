using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour {
    [SerializeField] private AssetReference descriptionPanel;
    [SerializeField] private TextMeshProUGUI stockText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private RawImage productImage;
    public IngredientSO ingredient;
    public ProductSO product;
    [HideInInspector] public DeliveryManager deliveryManager;

    public int nbIngredient = 0;

    void Start() {
        GetComponentInChildren<AmmountManager>().deliveryManager = deliveryManager;
        GetComponentInChildren<AmmountManager>().deliveryButton = this;
        nbIngredient = 0;
        if (ingredient) {
            stockText.SetText("Stock : " + ingredients.GetIngredientAmount(ingredient));
            priceText.SetText(ingredient.price + "€/U");
            productImage.texture = ingredient.image;
        }
        else if (product) {
            stockText.SetText(product.name);

            int totalPrice = 0;
            foreach (IngredientSO ingredient in product.ingredients)
                totalPrice += ingredient.price;

            priceText.SetText(totalPrice + "€/U");
            productImage.texture = product.image;
        }
    }

    public void UpdateStock() => stockText.text = "Stock : " + ingredients.GetIngredientAmount(ingredient);

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
    public void SetProduct(ProductSO product) => this.product = product;
}
