using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nameText;
    //[SerializeField] private AssetReference ammountPanelAsset;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private RawImage productImage;
    [SerializeField] private GameObject ammountPanel;

    private List<GameObject> ingredientButtons;
    private TextMeshProUGUI stockText;

    [HideInInspector] public IngredientSO ingredient;
    [HideInInspector] public ProductSO product;
    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public int nbIngredient = 0;

    void Start() {
        ingredientButtons = new List<GameObject>();
        nbIngredient = 0;

        GetComponentInChildren<AmmountManager>(true).deliveryManager = deliveryManager;
        GetComponentInChildren<AmmountManager>(true).deliveryButton = this;
    }

    public void SetIngredientButton(List<GameObject> buttons) => ingredientButtons = buttons;
    public DeliveryButton GetIngredientButton(IngredientSO ingredient) {
        for (int i = 0; i < ingredientButtons.Count; i++) {
            DeliveryButton button = ingredientButtons[i].GetComponent<DeliveryButton>();
            if (button.ingredient == ingredient)
                return button;
        }
        return null;
    }
    public void UpdateStock() {
     //   stockText.text = "Stock : " + ingredients.GetIngredientAmount(ingredient);
    }
    public void SetIngredientSO(ListIngredient ingredients) => this.ingredients = ingredients;

    public void SetIngredient(IngredientSO ingredient) {
        this.ingredient = ingredient;

        nameText.SetText(ingredient.name + " | " + ingredient.price + " /U");

        productImage.texture = ingredient.image;
    }

    public void DisplayAmount() {
        ammountPanel.SetActive(true);
    }

    public void SetProduct(ProductSO product) {
        this.product = product;

        if (stockText)
            stockText.SetText(product.name);

        int totalPrice = 0;
        foreach (IngredientSO ingredient in product.ingredients)
            totalPrice += ingredient.price;

        productImage.texture = product.image;
    }
}
