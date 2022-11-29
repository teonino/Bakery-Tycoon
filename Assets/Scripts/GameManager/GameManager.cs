using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [SerializeField] private bool debug;
    [SerializeField] private InputType inputType;
    [SerializeField] private AssetReference pausePanelAsset;
    [SerializeField] private int startingMoney;
    [SerializeField] private List<int> reputationExpToLvUp;
    [SerializeField] private List<ProductSO> productsList;
    [SerializeField] private List<StockIngredient> ingredientLists;
    [Header("Stocks")]
    [SerializeField] private int maxStock;

    [HideInInspector] public DayTime dayTime = DayTime.Morning;
    [HideInInspector] public int day = 1;

    private Action<int> updateMoneyUI;
    private Action<Reputation, int> updateReputationUI;
    private Dictionary<string, int> productPrices;
    private List<Delivery> deliveries;
    private PlayerController playerController;
    private DayStatistics dayStatistics;
    private GameObject lastButton;
    private GameObject pausePanel;
    private int money;
    private Reputation reputation;
    private int currentReputationLv;
    private int currentStock;

    private void Awake() {
        money = startingMoney;

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
        deliveries = new List<Delivery>();
        reputation = new Reputation();
    }

    private void Start() {
        //Set Input Device
        if (inputType == InputType.Gamepad && Gamepad.all.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Gamepad.all[0] };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
        }
        else if (inputType == InputType.KeyboardMouse && InputSystem.devices.Count > 0 && InputSystem.devices.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
        }

        //Set UI
        updateMoneyUI(money);
        updateReputationUI(reputation, reputationExpToLvUp[reputation.level]);
    }

    public void Pause() {
        if (!pausePanel)
            pausePanelAsset.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
                pausePanel = go.Result;
                pausePanel.SetActive(true);
            };
        else
            pausePanel.SetActive(true);
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
    public void AddDelivery(Delivery delivery) {
        deliveries.Add(delivery);

        if (delivery.day == 0)
            StartCoroutine(ExpressDelivery(delivery));

    }
    private IEnumerator ExpressDelivery(Delivery delivery) {
        yield return new WaitForSeconds(15);
        DeliverOrder(delivery);
    }

    private void DeliverOrder(Delivery delivery) {
        foreach (StockIngredient stockIngredient in GetIngredientList())
            foreach (StockIngredient deliveryIngredient in delivery.ingredients)
                if (stockIngredient.ingredient == deliveryIngredient.ingredient)
                    stockIngredient.amount += deliveryIngredient.amount;
        deliveries.Remove(delivery);
        print("Order delivered !");
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
    public float GetReputationExperience() => reputation.experience;
    public int GetReputationLevel() => reputation.level;
    public void AddReputation(int value) {
        reputation.experience += value;

        if (reputation.experience > reputationExpToLvUp[reputation.level]) {
            reputation.level++;
            reputation.experience = 0;
        }
        updateReputationUI(reputation, reputationExpToLvUp[reputation.level]);
    }
    public void RemoveReputation(int value) {
        reputation.experience -= value;
        if (reputation.experience < 0)
            reputation.experience = 0;
        updateReputationUI(reputation, reputationExpToLvUp[reputation.level]);
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
    public DayStatistics GetDayStatistics() => dayStatistics;
    public bool GetDebug() => debug;
}
