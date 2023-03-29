using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateQuest", menuName = "Quest/CreateQuest")]
public class CreateQuest : Quest
{
    [Header("Create Quest Parameters")]
    [SerializeField] private ProductSO product;
    [SerializeField] private int amount;
    [SerializeField] private LocalizedStringComponent localizedString;


    public void Init(ProductSO product, int amount)
    {
        this.product = product;
        this.amount = amount;
        this.title = $"Create {amount} {product.name}";
        this.currentAmount = 0;

        isActive = true;

        reward = RewardType.Money;
        rewardAmount = 25;
    }

    public ProductSO GetProduct() => product;

    public override int GetCurrentAmount() => currentAmount;
    public override int GetObjective() => amount;

    public void CheckProduct(ProductSO product)
    {
        if (isActive)
        {
            if (string.Equals(this.product.keyName, product.keyName))
            {
                currentAmount++;
            }

            if (currentAmount >= amount)
            {
                OnCompleted();
            }

            FindObjectOfType<StartingPanel>(true)?.UpdateUI();
        }
    }

    public void Completed()
    {
        OnCompleted();
    }
}
