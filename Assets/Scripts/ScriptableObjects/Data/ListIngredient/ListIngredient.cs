using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ListIngredient", menuName = "Data/ListIngredient")]
public class ListIngredient : Data
{
    [SerializeField] private List<StockIngredient> listIngredient;

    private void OnEnable() {
        ResetValues();
    }

    public override void ResetValues() {
        foreach (StockIngredient stock in listIngredient)
            stock.amount = 0;
    }

    public int GetIngredientLenght() => listIngredient.Count;
    public List<StockIngredient> GetIngredientList() =>listIngredient;

    public int GetIngredientAmount(IngredientSO ingredient) {
        int amount = 0;
        foreach(StockIngredient stock in listIngredient) {
            if(stock.ingredient == ingredient)
                amount = stock.amount;
        }
        return amount;
    }

    public void RemoveIngredientStock(IngredientSO ingredient, int amount) {
        foreach (StockIngredient stock in listIngredient)
            if (ingredient == stock.ingredient)
                stock.amount -= amount;
    }
}
