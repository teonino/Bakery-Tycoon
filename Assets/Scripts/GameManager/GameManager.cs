using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AssetReference pausePanelAsset;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private Controller controller;
    [SerializeField] private List<int> reputationExpToLvUp;
    [SerializeField] private int maxStock;
    [SerializeField] private int timeExpressDelivery;
    [SerializeField] private Day day;

    private Dictionary<string, int> productPrices;
    private List<Delivery> deliveries;
    private PlayerController playerController;
    private GameObject lastButton;
    private GameObject pausePanel;
    private int currentStock;

    private void Awake() {
        //Set product list for prices
        productPrices = new Dictionary<string, int>();
        foreach (ProductSO product in TmpBuild.instance.products.GetProductList())
            productPrices.Add(product.name, product.price);

        //Get player Controller
        playerController = FindObjectOfType<PlayerController>();

        //Set Statistic class
        deliveries = new List<Delivery>();

        day.AddEventOnNewDay(CheckDeliveries);
    }

    private void Start() {
        //Set Input Device
        if (TmpBuild.instance.controller.GetInputType() == InputType.Gamepad && Gamepad.all.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Gamepad.all[0] };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
        }
        else if (TmpBuild.instance.controller.GetInputType() == InputType.KeyboardMouse && InputSystem.devices.Count > 0 && InputSystem.devices.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
        }
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
        foreach (StockIngredient stock in TmpBuild.instance.ingredients.GetIngredientList())
            if (ingredient == stock.ingredient)
                return stock.amount;

        return -1;
    }

    public void RemoveIngredientStock(IngredientSO ingredient, int amount) {
        foreach (StockIngredient stock in TmpBuild.instance.ingredients.GetIngredientList())
            if (ingredient == stock.ingredient)
                stock.amount -= amount;
    }
    public void AddDelivery(Delivery delivery) {
        deliveries.Add(delivery);

        if (delivery.day == 0)
            StartCoroutine(ExpressDelivery(delivery));
    }

    private void CheckDeliveries() {
        List<Delivery> todayDeliveries = new List<Delivery>();

        //Fetch all deliveries arriving today
        foreach (Delivery delivery in deliveries) 
            if (delivery.day == TmpBuild.instance.day.GetDayCount())
                todayDeliveries.Add(delivery);

        //Add stock and Remove them from deliveries planned
        foreach (Delivery delivery in todayDeliveries)
            DeliverOrder(delivery);

    }

    private IEnumerator ExpressDelivery(Delivery delivery) {
        yield return new WaitForSeconds(timeExpressDelivery);
        DeliverOrder(delivery);
    }

    private void DeliverOrder(Delivery delivery) {
        foreach (StockIngredient stockIngredient in TmpBuild.instance.ingredients.GetIngredientList())
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
    public int GetProductPrice(ProductSO product) => productPrices[product.name];
    public int SetProductPrice(ProductSO product, int value) => productPrices[product.name] = value;
    public void AddProductSold(ProductSO product) => print("yes");//dayStatistics.AddProductSold(product);
    public float GetCurrentStock() => currentStock;
    public float GetMaxStock() => maxStock;
}
