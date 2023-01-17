using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientUnlocked", menuName = "Event/IngredientUnlocked")]
public class IngredientUnlockSO : ScriptableObject
{
    public Action<IngredientSO> action;

    public void Invoke(IngredientSO ingredient) => action?.Invoke(ingredient);
}
