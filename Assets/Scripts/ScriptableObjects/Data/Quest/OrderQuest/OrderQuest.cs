using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderQuest", menuName = "Quest/OrderQuest")]
public class OrderQuest : Quest {
    [Header("Order Quest Parameters")]
    [SerializeField] private IngredientSO ingredient;
    [SerializeField] private int amount;

    public void CheckOrder(IngredientSO ingredient, int amount) {
        if (isActive && this.ingredient == ingredient && this.amount >= amount)
            OnCompleted();
    }
}
