using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StockIngredient", menuName = "StockIngredient", order = 1)]
public class StockIngredientSO : ScriptableObject
{
    public IngredientSO ingredient;
    public int amount;
}
