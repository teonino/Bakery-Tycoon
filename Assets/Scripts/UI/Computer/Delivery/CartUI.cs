using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class CartUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI orderSumary;
    [SerializeField] private TextMeshProUGUI totalCostText;
    [SerializeField] private LocalizeStringEvent totalCostString;
    [SerializeField] private ListDeliveries deliveries;
    [SerializeField] private Money money;
    [SerializeField] private Day day;
    [SerializeField] private PlayerControllerSO playerController;

    protected Delivery delivery;

    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public Dictionary<IngredientSO, int> cart;
    [HideInInspector] public float cartWeight;
    [HideInInspector] public int cartCost;

    private float cost = 0; //Display value of a ingredient, not used yet
    private LocalizedString localizedString;
    private IntVariable localizedCartCost = null;

    private void Awake() {
        deliveries.SetDefaultExpressOrderTime();

        localizedString = totalCostString.StringReference;
        if (!localizedString.TryGetValue("totalCost", out IVariable value)) {
            localizedCartCost = new IntVariable();
            localizedString.Add("totalCost", localizedCartCost);
        }
        else {
            localizedCartCost = value as IntVariable;
        }
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
                newText += stock.Key.name + " x" + stock.Value + " : " + stock.Key.price * stock.Value + "\n";
                cost += stock.Key.price * stock.Value;
            }
            orderSumary.SetText(newText);
        }
        totalCostText.SetText("Total : " + cartCost);
        localizedCartCost.Value = cartCost;
    }

    public void ClearText() {
        orderSumary.SetText("");
        totalCostText.SetText("");
    }

    public virtual void Order(InputAction.CallbackContext ctx) {
        if (ctx.performed && cart != null) {
            //Check if the order can be bought
            if (cartCost <= money.GetMoney()) {
                delivery = new Delivery(day.GetCurrentDay());
                foreach (KeyValuePair<IngredientSO, int> stock in cart) {
                    if (stock.Value > 0) {
                        delivery.Add(stock.Key, stock.Value);
                    }
                }
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
