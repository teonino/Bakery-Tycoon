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
        title = "Talk to a regular customer";
        isActive = true;

        reward = RewardType.Reputation;
        rewardAmount = 5;
    }

    public bool OnInterract() {
        if (isActive) {
            if (spawnCustomer)
                FindObjectOfType<SpawnCustomer>()?.SpawnCustomerAsset(true, breadSO);
            OnCompleted();

            return true;
        }
        return false;
    }
}
