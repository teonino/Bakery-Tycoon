using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [SerializeField] private InputType inputType;
    [Space(10)]
    [SerializeField] private List<ProductSO> productsList;
    private Dictionary<string, int> productPrices;
    [Space(10)]
    [SerializeField] private List<StockIngredient> ingredientLists;

    [Header("Stocks")]
    [SerializeField] private int maxStock;

    public DayTime dayTime;
    
    private int currentStock;
    private PlayerController playerController;
    private Action<int> updateMoneyUI, updateReputationUI;
    private DayStatistics dayStatistics;
    private GameObject lastButton;
    private int money = 100;
    private int reputation;

    private void Awake() {
        //Set product list for prices
        productPrices = new Dictionary<string, int>();
        foreach (ProductSO product in productsList)
            productPrices.Add(product.name, product.price);

        //Get player Controller
        playerController = FindObjectOfType<PlayerController>();

        //Set UI textes
        updateMoneyUI = FindObjectOfType<MoneyUI>().SetMoney;
        updateReputationUI = FindObjectOfType<ReputationUI>().SetReputation;

        //Set Statistic class
        dayStatistics = new DayStatistics(this);
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

        updateMoneyUI(money);
        updateReputationUI(reputation);
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
    public float GetReputation() => reputation;
    public void AddReputation(int value) {
        reputation += value;
        updateReputationUI(reputation);
    }
    public float GetMoney() => money;
    public void AddMoney(int value) {
        money += value;
        dayStatistics.AddMoney(value);
        updateMoneyUI(money);
    }
    public void RemoveMoney(int value) {
        money -= value;
        dayStatistics.RemoveMoney(value);
        updateMoneyUI(money);
    }

    public void AddProductSold(ProductSO product) => dayStatistics.AddProductSold(product);
    public float GetCurrentStock() => currentStock;
    public float GetMaxStock() => maxStock;
}
