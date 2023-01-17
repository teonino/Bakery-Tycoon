using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientsForProduct
{
    public IngredientSO ingredient;
    private bool unlocked = false;

    public bool isUnlocked() => unlocked;
}
