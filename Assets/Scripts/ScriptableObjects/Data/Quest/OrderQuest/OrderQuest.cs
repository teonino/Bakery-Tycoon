using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderQuest", menuName = "Quest/OrderQuest")]
public class OrderQuest : Quest {
    [Header("Order Quest Parameters")]
    [SerializeField] private IngredientSO ingredient;
    [SerializeField] private List<StockIngredient> ingredients;

    private int nbIngredientMatched = 0;

    public void CheckOrder(Delivery delivery) {
        foreach (StockIngredient expectedIngredient in ingredients)
            foreach (StockIngredient ingredient in delivery.GetIngredients())
                if (expectedIngredient == ingredient)
                    nbIngredientMatched++;

        if (isActive && nbIngredientMatched == ingredients.Count)
            OnCompleted();
    }

    public void CheckIngredient(IngredientSO ingredient) {
        if (isActive && ingredient == this.ingredient)
            OnCompleted();
    }
}
