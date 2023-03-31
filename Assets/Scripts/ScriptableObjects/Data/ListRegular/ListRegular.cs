using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListRegularSO", menuName = "Data/ListRegular")]
public class ListRegular : Data {
    [SerializeField] private ListIngredient listIngredient;
    [SerializeField] private IngredientUnlockSO ingredientUnlock;
    [SerializeField] private NotificationEvent notifEvent;
    [SerializeField] private NotificationType notifType;
    int totalFriendship;
    int lastRank;

    private List<StockIngredient> stockIngredients;

    public override void ResetValues() {
        totalFriendship = 0;
        lastRank = -1;
        stockIngredients = listIngredient.GetIngredientList();
    }

    public void AddFriendship(int i) {
        totalFriendship += i;
        if (totalFriendship < 0)
            totalFriendship = 0;

        if (totalFriendship % 3 == 0 && totalFriendship >= lastRank) {
            bool unlocked = false;
            for(int j = 0; j < stockIngredients.Count && !unlocked; j++) {
                if (!stockIngredients[j].ingredient.unlocked) {
                    stockIngredients[j].ingredient.unlocked = true;
                    ingredientUnlock.Invoke(stockIngredients[j].ingredient); //Update ingredient available everywhere
                    notifEvent.Invoke(notifType);
                    unlocked = true;
                }
            }
            lastRank = totalFriendship;
        }
    }
}
