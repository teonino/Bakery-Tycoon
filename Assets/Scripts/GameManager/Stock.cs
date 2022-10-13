using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stock 
{
    public IngredientSO ingredient;
    public int amount;

    public Stock(IngredientSO ingredient, int amount) {
        this.ingredient = ingredient;
        this.amount = amount;
    }
}
