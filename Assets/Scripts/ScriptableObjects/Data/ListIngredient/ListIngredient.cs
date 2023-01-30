using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ListIngredient", menuName = "Data/ListIngredient")]
public class ListIngredient : Data {
    [SerializeField] private List<StockIngredient> tutoListIngredient;
    [SerializeField] private List<StockIngredient> defaultListIngredient;
    [SerializeField] private List<StockIngredient> listIngredient;
    [SerializeField] private Tutorial tutorial;

    private void OnEnable() {
        ResetValues();
    }

    public override void ResetValues() {
        for (int i = 0; i < listIngredient.Count; i++) {
            listIngredient[i].amount = 0;
            listIngredient[i].ingredient.unlocked = false;
            if (tutorial.GetTutorial()) {
                for (int j = 0; j < tutoListIngredient.Count; j++) {
                    if (listIngredient[i].ingredient == tutoListIngredient[j].ingredient)
                        listIngredient[i].ingredient.unlocked = true;
                }
            }
            else {
                for (int j = 0; j < defaultListIngredient.Count; j++) {
                    if (listIngredient[i].ingredient == defaultListIngredient[j].ingredient)
                        listIngredient[i].ingredient.unlocked = true;
                }
            }
        }
    }

    public int GetIngredientLenght() => listIngredient.Count;
    public List<StockIngredient> GetIngredientList() => listIngredient;

    public int GetIngredientAmount(IngredientSO ingredient) {
        int amount = 0;
        foreach (StockIngredient stock in listIngredient) {
            if (stock.ingredient == ingredient)
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
