using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ListIngredient", menuName = "Data/ListIngredient")]
public class ListIngredient : Data
{
    [SerializeField] private List<StockIngredient> tutoListIngredient;
    [SerializeField] private List<StockIngredient> defaultListIngredient;
    [SerializeField] private List<StockIngredient> listIngredient;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private bool debug;

    public override void ResetValues()
    {
        for (int i = 0; i < listIngredient.Count; i++)
        {
            listIngredient[i].amount = 0;
            listIngredient[i].ingredient.unlocked = false;
            listIngredient[i].ingredient.SetName();
        }

        if (tutorial.GetTutorial())
        {
            for (int i = 0; i < tutoListIngredient.Count; i++)
            {
                tutoListIngredient[i].ingredient.unlocked = true;
            }
        }
        else if (debug)
        {
            for (int i = 0; i < listIngredient.Count; i++)
            {
                listIngredient[i].ingredient.unlocked = true;
            }
        }
        else
        {
            for (int i = 0; i < defaultListIngredient.Count; i++)
            {
                defaultListIngredient[i].ingredient.unlocked = true;
            }
        }
    }


    public void UnlockIngredient()
    {
        bool smthUnlock = false;
        for (int i = 0; i < listIngredient.Count && !smthUnlock; i++)
        {
            if (!listIngredient[i].ingredient.unlocked)
            {
                listIngredient[i].ingredient.unlocked = true;
                smthUnlock = true;
            }
        }
    }

    public int GetIngredientLenght() => listIngredient.Count;
    public List<StockIngredient> GetIngredientList() => listIngredient;

    public int GetIngredientAmount(IngredientSO ingredient)
    {
        int amount = 0;
        foreach (StockIngredient stock in listIngredient)
        {
            if (stock.ingredient == ingredient)
                amount = stock.amount;
        }
        return amount;
    }

    public void RemoveIngredientStock(IngredientSO ingredient, int amount)
    {
        foreach (StockIngredient stock in listIngredient)
            if (ingredient == stock.ingredient)
                stock.amount -= amount;
    }
}
