using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
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
    [SerializeField] private TruckDelivery truckReference;
    [SerializeField] private Image holdFeedbackGO;

    protected Delivery delivery;

    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public Dictionary<IngredientSO, int> cart;
    [HideInInspector] public float cartWeight;
    [HideInInspector] public int cartCost;

    private Coroutine coroutine;
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
        playerController.GetPlayerController().playerInput.Amafood.OrderAndClear.started += FeedbackHold;
        playerController.GetPlayerController().playerInput.Amafood.OrderAndClear.performed += Order;
    }

    private void OnDisable() {
        playerController.GetPlayerController().playerInput.Amafood.OrderAndClear.started -= FeedbackHold;
        playerController.GetPlayerController().playerInput.Amafood.OrderAndClear.performed -= Order;
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
        //localizedCartCost.Value = cartCost;
    }

    public void ClearText() {
        orderSumary.SetText("");
        totalCostText.SetText("Total: ");
    }

    public virtual void FeedbackHold(InputAction.CallbackContext ctx) {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(FillHoldFeedback());
    }

    private IEnumerator FillHoldFeedback() {
        holdFeedbackGO.fillAmount += 0.025f;
        yield return new WaitForEndOfFrame();
        if (holdFeedbackGO.fillAmount >= 1)
            holdFeedbackGO.fillAmount = 0;
        else
            coroutine = StartCoroutine(FillHoldFeedback());
    }

    public virtual void Order(InputAction.CallbackContext ctx) {
        if (coroutine != null)
            StopCoroutine(coroutine);
        holdFeedbackGO.fillAmount = 0;


        if (ctx.interaction.ToString().Contains("Hold")) {
            Clear();
        }
        else {
            if (ctx.performed && cart != null) {
                if (cart.Count > 0) {
                    //Check if the order can be bought
                    if (cartCost <= money.GetMoney()) {
                        if (truckReference.CanAddDelivery()) {
                            delivery = new Delivery(day.GetDayCount());
                            foreach (KeyValuePair<IngredientSO, int> stock in cart) {
                                if (stock.Value > 0) {
                                    delivery.Add(stock.Key, stock.Value);
                                }
                            }

                            FindObjectOfType<Computer>().StartCoroutine(deliveries.ExpressDelivery(delivery));

                            deliveries.Add(delivery);
                            money.AddMoney(-cartCost);
                            Clear();
                        }
                        else
                            print("Truck not available at the moment");
                    }
                    else
                        print("Not enought money");
                }
            }
        }
    }

    public void Clear() {
        cartCost = 0;
        if (deliveryManager)
            deliveryManager.ResetCart(true);
    }
}
