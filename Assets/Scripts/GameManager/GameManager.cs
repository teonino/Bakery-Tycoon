using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Global variables")]
    [HideInInspector]
    public PlayerController playerController;
    public DayTime dayTime;
    public float reputation = 0;
    public float money = 0;

    [Space(10)]
    public List<ProductSO> productsList;
    [Space(10)]
    public List<StockIngredient> ingredientLists;

    [Header("Stocks")]
    public int maxStock;
    public int currentStock;

    private void Start() {
        playerController = FindObjectOfType<PlayerController>();

        //ONLY FOR UNITY USES, REMOVE FOR BUILD
        foreach (StockIngredient stockIngredient in ingredientLists) 
            stockIngredient.amount = 0;
        foreach (ProductSO product in productsList) 
            product.price = product.initialPrice;
        
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

    public int GetLenghtProducts() => productsList.Count;
    public int GetLenghtIngredients() => ingredientLists.Count;
}
