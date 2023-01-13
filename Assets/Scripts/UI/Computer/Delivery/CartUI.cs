using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CartUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI orderSumary;
    [SerializeField] private TextMeshProUGUI totalCostText;
    [SerializeField] private ListDeliveries deliveries;
    [SerializeField] private Money money;
    [SerializeField] private Day day;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private PlayerControllerSO playerController;
    [SerializeField] private OrderTypeQuest orderTypeQuest;

    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public Dictionary<IngredientSO, int> cart;
    [HideInInspector] public float cartWeight;
    [HideInInspector] public int cartCost;

    private float cost = 0; //Display value of a ingredient, not used yet

    private void Awake() {
        if (tutorial)
            deliveries.SetExpressOrderTime(0);
        else
            deliveries.SetDefaultExpressOrderTime();
    }

    private void OnEnable() {
        playerController.GetPlayerController().playerInput.Amafood.Order.performed += Order;
    }

    private void OnDisable() {
        playerController.GetPlayerController().playerInput.Amafood.Order.performed -= Order;
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
        totalCostText.SetText("Total : " + cartCost + "€");
    }

    public void ClearText() {
        orderSumary.SetText("");
        totalCostText.SetText("");
    }

    public void Order(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            //Check if the order can be bought
            if (cartCost <= money.GetMoney()) {
                Delivery delivery = new Delivery(day.GetCurrentDay());
                foreach (KeyValuePair<IngredientSO, int> stock in cart) {
                    if (stock.Value > 0) {
                        delivery.Add(stock.Key, stock.Value);
                    }
                }

                orderTypeQuest?.CheckDeliveryType();

                //Express deliveries
                if (delivery.GetDay() == day.GetCurrentDay())
                    StartCoroutine(deliveries.ExpressDelivery(delivery));

                deliveries.Add(delivery);
                money.AddMoney(-cartCost);
                Clear();
            }
        }
    }

    public void Clear() {
        cartCost = 0;
        if (deliveryManager)
            deliveryManager.ResetCart();
    }
}
