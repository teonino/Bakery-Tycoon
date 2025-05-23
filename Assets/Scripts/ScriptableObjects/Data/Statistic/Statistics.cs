﻿using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistic", menuName = "Data/Statistic")]
public class Statistics : ScriptableObject {
    [SerializeField] private ListProduct products;
    private Dictionary<ProductSO, int> productsSold;
    private int moneySpent;
    private int moneyEarned;

    public void OnEnable() {
        moneyEarned = moneySpent = 0;
        productsSold = new Dictionary<ProductSO, int>();
        foreach (ProductSO product in products.GetProductList())
            productsSold.Add(product, 0);
    }

    public KeyValuePair<string,int> GetMostProductSold() {
        string product = "None";
        int productAmount = 0;

        foreach (KeyValuePair<ProductSO, int> productSold in productsSold) {
            if (product == "None") {
                product = productSold.Key.name;
                productAmount = productSold.Value;
            }

            if(productAmount < productSold.Value) {
                product = productSold.Key.name;
                productAmount = productSold.Value;
            }
        }
        if(GetProductsSold() != 0)
        productAmount = productAmount / GetProductsSold();

        return new KeyValuePair<string, int>(product, productAmount);
    }

    public double GetPercentageAmongAllProduct() {
        if (GetProductsSold() == 0)
            return 0;
        return GetMostProductSold().Value / GetProductsSold() * 100;
    }

    public int GetProductsSold() {
        int total = 0;
        foreach (KeyValuePair<ProductSO, int> productSold in productsSold) {
            total += productSold.Value;
        }
        return total;
    }

    public void AddProductSold(ProductSO product) {
        if (productsSold.ContainsKey(product)) {
            Console.WriteLine(product.name + " / " + productsSold[product]);
            productsSold[product]++;
        }
    }

    public StockIngredient GetLowestIngredient(ListIngredient ingredients) {
        StockIngredient lowestIngredient = null;

        foreach (StockIngredient ingredient in ingredients.GetIngredientList()) {
            if (lowestIngredient == null) lowestIngredient = ingredient;
            if (ingredient.amount < lowestIngredient.amount) lowestIngredient = ingredient;
        }
        return lowestIngredient;
    }

    public void AddMoney(int value) => moneyEarned += value;
    public void RemoveMoney(int value) => moneySpent += value;
    public int GetMoneyEarned() => moneyEarned; 
    public int GetMoneySpent() => moneySpent; 
}