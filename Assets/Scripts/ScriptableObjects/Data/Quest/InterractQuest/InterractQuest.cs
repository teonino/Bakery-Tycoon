using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InterractQuest", menuName = "Quest/InterractQuest")]
public class InterractQuest : Quest {
    [SerializeField] private bool spawnCustomer = false;
    [SerializeField] private ProductSO breadSO;

    public void Init() {
        variable = "";
        isActive = true;
        this.currentAmount = 0;

        reward = RewardType.Reputation;
        rewardAmount = 5;
    }

    public override int GetCurrentAmount()
    {
        if (isActive)
            return 0;
        else
            return 1;
    }
    public override int GetObjective() => 1;

    public bool OnInterract() {
        if (isActive) {
            if (spawnCustomer)
                FindObjectOfType<SpawnCustomer>()?.SpawnCustomerAsset(true, breadSO);

            OnCompleted();
            FindObjectOfType<StartingPanel>(true)?.UpdateUI();

            return true;
        }
        return false;
    }
}
