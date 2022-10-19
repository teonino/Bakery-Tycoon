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
        //Check if the order can be stocked
        if (cartWeight > 0 && cartWeight + gameManager.currentStock <= gameManager.maxStock) {
            foreach (KeyValuePair<IngredientSO, int> stock in cart) {
                if (stock.Value > 0) {
                    foreach (StockIngredient stockIngredient in gameManager.ingredientLists)
                        if (stockIngredient.ingredient == stock.Key)
                            stockIngredient.amount += stock.Value;
                }
            }

            deliveryManager.Reset();
        }
    }
}
