using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateQuest", menuName = "Quest/CreateQuest")]
public class CreateQuest : Quest {
    [Header("Create Quest Parameters")]
    [SerializeField] private ProductSO product;
    [SerializeField] private int amount;

    private int currentAmount = 0;

    public void CheckProduct(ProductSO product) {
        if (isActive) {
            if (this.product == product) {
                currentAmount++;
            }

            if (currentAmount > amount) {
                OnCompleted();
            }
        }
    }
}
