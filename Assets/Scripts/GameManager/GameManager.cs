using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [Header("Global variables")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InputType inputType;
    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private TextMeshProUGUI reputationTxt;

    [Header("Day Variable")]
    [SerializeField] private DayTime dayTime;
    [SerializeField] private TextMeshProUGUI dayTimeTxt;

    [Header("Products & Ingredients list")]
    [Space(10)]
    [SerializeField] private List<ProductSO> productsList;
    private Dictionary<string, int> productPrices;
    [Space(10)]
    [SerializeField] private List<StockIngredient> ingredientLists;

    [Header("Stocks")]
    [SerializeField] private int maxStock;
    [SerializeField] private int currentStock;

    private GameObject lastButton;
    private float money = 100;
    private float reputation;

    private void Awake() {
        productPrices = new Dictionary<string, int>();
        foreach (ProductSO product in productsList)
            productPrices.Add(product.name, product.price);

        playerController = FindObjectOfType<PlayerController>();

        moneyTxt.SetText(money + "€");
        dayTimeTxt.SetText(GetDayTxt());
        reputationTxt.SetText("Reputation : " + reputation);

        //ONLY FOR UNITY USES, REMOVE FOR BUILD
        foreach (StockIngredient stockIngredient in ingredientLists)
            stockIngredient.amount = 0;
    }

    private void Start() {
        if (inputType == InputType.Gamepad && Gamepad.all.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Gamepad.all[0] };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
        }
        else if (inputType == InputType.KeyboardMouse && InputSystem.devices.Count > 0 && InputSystem.devices.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
        }
    }

    private string GetDayTxt() {
        string s = "";
        switch (dayTime) {
            case DayTime.Morning:
                s = "Morning";
                break;
            case DayTime.Day:
                s = "Day";
                break;
            case DayTime.Evening:
                s = "Evening";
                break;
            default:
                break;
        }
        return s;
    }

    public void SetDayTime() {
        dayTime++;
        dayTimeTxt.SetText(GetDayTxt());
    }

    public int GetIngredientAmount(IngredientSO ingredient) {
        foreach (StockIngredient stock in ingredientLists)
            if (ingredient == stock.ingredient)
                return stock.amount;

        return -1;
    }

    public void RemoveIngredientStock(IngredientSO ingredient, int amount) {
        foreach (StockIngredient stock in ingredientLists)
            if (ingredient == stock.ingredient)
                stock.amount -= amount;
    }

    public void SetEventSystemToStartButton(GameObject startButton) {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void RegisterCurrentSelectedButton() {
        lastButton = EventSystem.current.currentSelectedGameObject;
    }

    public void SetEventSystemToLastButton() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButton);
    }

    public PlayerController GetPlayerController() => playerController;
    public List<ProductSO> GetProductList() => productsList;
    public int GetProductPrice(ProductSO product) => productPrices[product.name];
    public int SetProductPrice(ProductSO product, int value) => productPrices[product.name] = value;
    public List<StockIngredient> GetIngredientList() => ingredientLists;
    public int GetProductsLenght() => productsList.Count;
    public int GetIngredientsLenght() => ingredientLists.Count;
    public InputType GetInputType() => inputType;
    public InputType SetInputType(InputType value) => inputType = value;
    public bool IsGamepad() => inputType == InputType.Gamepad;

    public DayTime GetDayTime() => dayTime;
    public float GetReputation() => reputation;
    public void AddReputation(float value) {
        reputation += value;
        reputationTxt.SetText("Reputation : " + reputation);
    }
    public float GetMoney() => money;
    public void AddMoney(float value) {
        money += value;
        moneyTxt.SetText(money + "€");
    }
    public void RemoveMoney(float value) {
        money -= value;
        moneyTxt.SetText(money + "€");
    }
    public float GetCurrentStock() => currentStock;
    public float GetMaxStock() => maxStock;
}
