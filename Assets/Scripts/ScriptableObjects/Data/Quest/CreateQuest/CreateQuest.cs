using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateQuest", menuName = "Quest/CreateQuest")]
public class CreateQuest : Quest {
    [Header("Create Quest Parameters")]
    [SerializeField] private ProductSO product;
    [SerializeField] private int amount;


    public void Init(ProductSO product, int amount) {
        this.product = product;
        this.amount = amount;
        this.title = $"Create {amount} {product.name}";

        isActive = true;

        reward = RewardType.Money;
        rewardAmount = 25;
    }

    public ProductSO GetProduct() => product;

    public void CheckProduct(ProductSO product) {
        if (isActive) {
            if (string.Equals(this.product.name,product.name)) {
                currentAmount++;
            }

            if (currentAmount >= amount) {
                OnCompleted();
            }
        }
    }

    public void Completed() {
        OnCompleted();
    }
}
