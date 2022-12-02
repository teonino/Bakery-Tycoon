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
    [HideInInspector] public DeliveryManager deliveryManager;

    public int nbIngredient = 0;

    void Start() { 
        GetComponentInChildren<AmmountManager>().deliveryManager = deliveryManager;
        GetComponentInChildren<AmmountManager>().deliveryButton = this;
        nbIngredient = ingredients.GetIngredientAmount(ingredient);
        stockText.SetText("Stock :" + nbIngredient);
        priceText.SetText(ingredient.price + "€/U");
        productImage.texture = ingredient.image;
    }

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
}
