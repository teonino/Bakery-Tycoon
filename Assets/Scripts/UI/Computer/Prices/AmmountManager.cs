using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmountManager : MonoBehaviour {
    [SerializeField] private int amountToBuy;
    [SerializeField] private TextMeshProUGUI textAmmount;
    [SerializeField] public DeliveryButton deliveryButton;
    public DeliveryManager deliveryManager;

    private void Start() {
        deliveryManager = FindObjectOfType<DeliveryManager>();
        amountToBuy = deliveryButton.nbIngredient;
    }

    public void MinusButtonIsClicked() {
        if (amountToBuy > 0) {
            amountToBuy -= 1;
            SetIngredientsInCart(false);
        }
    }

    public void PlusButtonIsClicked() {
        amountToBuy += 1;
        SetIngredientsInCart(true);
    }

    private void SetIngredientsInCart(bool add) {
        if (deliveryButton.ingredient)
            deliveryManager.SetIngredient(deliveryButton.ingredient, add);
        else
            foreach (IngredientSO ingredient in deliveryButton.product.ingredients) {
                deliveryManager.SetIngredient(ingredient, add);
                deliveryButton.GetIngredientButton(ingredient).GetComponentInChildren<AmmountManager>().SetTextAmount();
            }
        
        textAmmount.text = amountToBuy.ToString();
    }

    public void SetTextAmount() {
        amountToBuy++;
        textAmmount.text = amountToBuy.ToString();
    }

    public int GetAmount() => amountToBuy;
    public void ResetAmount() {
        amountToBuy = 0;
        textAmmount.text = amountToBuy.ToString();
    }
}
