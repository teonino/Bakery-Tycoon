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
    public Dictionary<IngredientSO, int> stocks;

    private GameManager gameManager;
    private float cost = 0;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        string newText = "";
        foreach (KeyValuePair<IngredientSO, int> stock in stocks) {
            if (stock.Value > 0) {
                newText += stock.Key.name + " x" + stock.Value + " : " + stock.Key.price * stock.Value + "€\n";
                cost += stock.Key.price * stock.Value;
            }
            orderSumary.SetText(newText);
        }
        totalCostText.SetText(cost + "€");
    }

    public void Order() {
        if (gameManager.money >= cost)
            foreach (KeyValuePair<IngredientSO, int> stock in stocks)
                if (stock.Value > 0)
                    foreach (StockIngredientSO stockIngredient in gameManager.ingredientList)
                        if (stockIngredient.ingredient == stock.Key)
                            stockIngredient.amount += stock.Value;
        DestroyPanel();
    }

    public void Quit(InputAction.CallbackContext context) {
        DestroyPanel();
    }

    private void DestroyPanel() {
        gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
        gameManager.playerController.playerInput.UI.Quit.performed += deliveryManager.Quit;
        if (gameObject)
            Addressables.ReleaseInstance(gameObject);
    }
}
