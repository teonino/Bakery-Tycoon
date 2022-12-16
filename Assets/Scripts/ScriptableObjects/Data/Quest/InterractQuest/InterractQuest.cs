using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InterractQuest", menuName = "Quest/InterractQuest")]
public class InterractQuest : Quest {
    [SerializeField] bool spawnCustomer = false;
    [SerializeField] ProductSO breadSO;

    public void OnInterract() {
        if (isActive) {
            if (spawnCustomer)
                FindObjectOfType<SpawnCustomer>()?.SpawnCustomerAsset(true, breadSO);
            OnCompleted();
        }
    }
}
