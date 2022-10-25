using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.HID.HID;

public class Cart : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI orderSumary;
    [SerializeField] private TextMeshProUGUI totalCostText;
    [SerializeField] private Button orderButton;

    [HideInInspector]
    public DeliveryManager deliveryManager;
    public Dictionary<IngredientSO, int> cart;
    public float cartWeight;
    public float cartCost;

    private GameManager gameManager;
    private float cost = 0;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void InitCart() {
        string newText = "";
        foreach (KeyValuePair<IngredientSO, int> stock in cart) {
            if (stock.Value > 0) {
                newText += stock.Key.name + " x" + stock.Value + " : " + stock.Key.price * stock.Value + "€\n";
                cost += stock.Key.price * stock.Value;
            }
            orderSumary.SetText(newText);
        }
    }

    public void ClearText() => orderSumary.text = "";

    public void Order() {
        //Check if the order can be stocked && bought
        if (cartWeight > 0 && cartWeight + gameManager.GetCurrentStock() <= gameManager.GetMaxStock()) {
            if (cartCost < gameManager.GetMoney()) {
                foreach (KeyValuePair<IngredientSO, int> stock in cart) {
                    if (stock.Value > 0) {
                        foreach (StockIngredient stockIngredient in gameManager.GetIngredientList())
                            if (stockIngredient.ingredient == stock.Key)
                                stockIngredient.amount += stock.Value;
                    }
                }
                gameManager.RemoveMoney(cartCost);
                Clear();
            }          
        }
    }

    public void Clear() => deliveryManager.ResetCart();
}
