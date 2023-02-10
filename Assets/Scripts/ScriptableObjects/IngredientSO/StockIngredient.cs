using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StockIngredient 
{
    public IngredientSO ingredient;
    public int amount;

    public StockIngredient(IngredientSO ingredient, int amount) {
        this.ingredient = ingredient;
        this.amount = amount;
    }

    public StockIngredient(IngredientSO ingredient)
    {
        this.ingredient = ingredient;
        this.amount = 0;
    }
}
