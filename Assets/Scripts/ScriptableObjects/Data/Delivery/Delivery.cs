using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery {
    private List<StockIngredient> ingredients;
    private int day;

    public Delivery(int day) { 
        ingredients = new List<StockIngredient>();
        this.day = day;
    }

    public void Add(IngredientSO ingredient, int nb) {
        bool alreadyOrdered = false;

        foreach (StockIngredient stock in ingredients) {
            if (stock.ingredient == ingredient) {
                stock.amount += nb;
                alreadyOrdered = true;
            }
        }
        if (!alreadyOrdered)
            ingredients.Add(new StockIngredient(ingredient, nb));
    }

    public List<StockIngredient> GetIngredients() => ingredients;
    public int GetDay() => day;
}
