using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private AssetReference descriptionPanel;
     public IngredientSO ingredient;
    [SerializeField] private TextMeshProUGUI stockText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RawImage productImage;
    [HideInInspector] public DeliveryManager deliveryManager;

    public int nbIngredient = 0;
    private GameManager gameManager;

    void Start() {  
        gameManager = FindObjectOfType<GameManager>();
        GetComponentInChildren<AmmountManager>().deliveryManager = deliveryManager;
        GetComponentInChildren<AmmountManager>().deliveryButton = this;
        nbIngredient = gameManager.GetIngredientAmount(ingredient);
        stockText.SetText("Stock :" + nbIngredient);
        stockText.SetText("Stock :" + ingredient.price);
        productImage.texture = ingredient.image;
    }

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
}
