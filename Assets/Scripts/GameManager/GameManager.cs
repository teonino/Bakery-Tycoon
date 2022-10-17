using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Global variables")]
    public PlayerController playerController;
    public DayTime dayTime;
    public float reputation = 0;
    public float money = 0;

    [Space(10)]
    public List<ProductSO> productsList;
    [Space(10)]
    public List<StockIngredientSO> ingredientList;

    [Header("Stocks")]
    public int maxStock;
    public int currentStock;

    private void Start() {
        playerController = FindObjectOfType<PlayerController>();

        //ONLY FOR UNITY USES, REMOVE FOR BUILD
        foreach (StockIngredientSO stockIngredient in ingredientList) {
            stockIngredient.amount = 0;
        }
    }

    public int GetLenghtProducts() => productsList.Count;
    public int GetLenghtIngredients() => ingredientList.Count;
}
