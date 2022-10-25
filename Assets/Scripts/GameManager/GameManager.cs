using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Global variables")]
    [HideInInspector]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float reputation;
    [SerializeField] private float money;
    [SerializeField] private TextMeshProUGUI moneyTxt;

    [Header("Day Variable")]
    [SerializeField] private DayTime dayTime;
    [SerializeField] private TextMeshProUGUI dayTimeTxt;

    [Header("Products & Ingredients list")]
    [Space(10)]
    [SerializeField] private List<ProductSO> productsList;
    [Space(10)]
    [SerializeField] private List<StockIngredient> ingredientLists;

    [Header("Stocks")]
    [SerializeField] private int maxStock;
    [SerializeField] private int currentStock;

    private void Start() {
        playerController = FindObjectOfType<PlayerController>();

        moneyTxt.SetText(money + "€");
        dayTimeTxt.SetText(GetDayTxt());

        //ONLY FOR UNITY USES, REMOVE FOR BUILD
        foreach (StockIngredient stockIngredient in ingredientLists)
            stockIngredient.amount = 0;
        foreach (ProductSO product in productsList)
            product.price = product.initialPrice;
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

    public PlayerController GetPlayerController() => playerController;
    public List<ProductSO> GetProductList() => productsList;
    public List<StockIngredient> GetIngredientList() => ingredientLists;
    public int GetProductsLenght() => productsList.Count;
    public int GetIngredientsLenght() => ingredientLists.Count;

    public DayTime GetDayTime() => dayTime;
    public float GetReputation() => reputation;
    public void AddReputation(float value) {
        reputation += value;
        //update ui
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
