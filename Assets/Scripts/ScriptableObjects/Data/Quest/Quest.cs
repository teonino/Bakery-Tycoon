using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
public abstract class Quest : ScriptableObject {
    [Header("Global Quest Parameters")]
    [SerializeField] protected string title;
    [SerializeField] private bool randomize;
    [SerializeField] protected RewardType reward;
    [SerializeField] protected int rewardAmount;
    [SerializeField] private Money money;
    [SerializeField] private Reputation reputation;
    [SerializeField] private NotificationEvent notifEvent;
    [SerializeField] private NotificationType notifType;
    [SerializeField] protected bool isActive = false;
    [Header("Unlock parameters")]
    [SerializeField] private ProductUnlockedSO productUnlocked;
    [SerializeField] private IngredientUnlockSO ingredientUnlocked;
    [SerializeField] private bool unlockingSmth;
    [SerializeField] private List<IngredientSO> ingredientsToUnlock;
    [SerializeField] private List<ProductSO> productsToUnlock;

    protected int currentAmount = 0;
    protected int objectiveAmount = 1;

    public Action OnCompletedAction;
    public Action<ProductSO> SpawnCustomer;

    protected enum RewardType { Reputation, Money }
    public void SetActive(bool active) => isActive = active;
    public void UpdateUI(TextMeshProUGUI text) => text.text = title;
    public string GetTitle() => title;
    public virtual int GetCurrentAmount() => currentAmount;
    public virtual int GetObjective() => objectiveAmount;
    public bool IsActive() => isActive;

    protected void OnCompleted() {
        switch (reward) {
            case RewardType.Money:
                money.AddMoney(rewardAmount);
                break;
            case RewardType.Reputation:
                reputation.AddReputation(rewardAmount);
                break;
        }


        if (unlockingSmth) {
            if (ingredientsToUnlock != null)
                foreach (IngredientSO ingredient in ingredientsToUnlock) {
                    ingredient.unlocked = true;
                    ingredientUnlocked.Invoke(ingredient);
                }
            if (productsToUnlock != null)
                foreach (ProductSO product in productsToUnlock) {
                    product.unlocked = true;
                    productUnlocked.Invoke(product);
                }
        }

        isActive = false;
        OnCompletedAction?.Invoke();
        notifEvent?.Invoke(notifType);
    }
}
