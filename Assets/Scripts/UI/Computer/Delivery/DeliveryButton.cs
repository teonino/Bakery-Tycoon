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
    private List<GameObject> ingredientButtons;
    public IngredientSO ingredient;
    public ProductSO product;
    [HideInInspector] public DeliveryManager deliveryManager;

    public int nbIngredient = 0;

    void Start() {
        ingredientButtons = new List<GameObject>();

        GetComponentInChildren<AmmountManager>().deliveryManager = deliveryManager;
        GetComponentInChildren<AmmountManager>().deliveryButton = this;
        nbIngredient = 0;
    }

    public void SetIngredientButton(List<GameObject> buttons) => ingredientButtons = buttons;
    public DeliveryButton GetIngredientButton(IngredientSO ingredient) {
        for(int i = 0; i < ingredientButtons.Count; i++) {
            DeliveryButton button = ingredientButtons[i].GetComponent<DeliveryButton>();
            if (button.ingredient == ingredient)
                return button;
        }
        return null;
    }
    public void UpdateStock() => stockText.text = "Stock : " + ingredients.GetIngredientAmount(ingredient);

    public void SetIngredient(IngredientSO ingredient) {
        this.ingredient = ingredient;

        stockText.SetText("Stock : " + ingredients.GetIngredientAmount(ingredient));
        priceText.SetText(ingredient.price + "€/U");
        productImage.texture = ingredient.image;
    }
    public void SetProduct(ProductSO product) {
        this.product = product;

        stockText.SetText(product.name);

        int totalPrice = 0;
        foreach (IngredientSO ingredient in product.ingredients)
            totalPrice += ingredient.price;

        priceText.SetText(totalPrice + "€/U");
        productImage.texture = product.image;
    }
}
