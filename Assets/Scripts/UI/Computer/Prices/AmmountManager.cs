using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmountManager : MonoBehaviour {
    [SerializeField] private IngredientDescription ingredientDescription;
    [SerializeField] private int amountToBuy;
    [SerializeField] private TextMeshProUGUI textAmmount;
    [SerializeField] private DeliveryManager deliveryManager;

    private void Start() {
        deliveryManager = FindObjectOfType<DeliveryManager>();
        amountToBuy = ingredientDescription.nbIngredient;
    }

    public void MinusButtonIsClicked() {
        if (amountToBuy > 0) {
            amountToBuy -= 1;
            SetIngredientsInCart();
        }
    }

    public void PlusButtonIsClicked() {
        amountToBuy += 1;
        SetIngredientsInCart();
    }

    private void SetIngredientsInCart() {
        deliveryManager.SetIngredient(ingredientDescription.ingredient, amountToBuy);
        textAmmount.text = amountToBuy.ToString();
    }

    public int GetAmount() => amountToBuy;
    public void ResetAmount() {
        amountToBuy = 0;
        textAmmount.text = amountToBuy.ToString();
    }
}
