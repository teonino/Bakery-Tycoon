using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangePriceQuest", menuName = "Quest/ChangePriceQuest")]
public class ChangePriceQuest : Quest {
    [SerializeField] private ProductSO breadSO;
    public void CheckPrice(int oldPrice, int newPrice) {
        if (isActive && oldPrice != newPrice) {
            OnCompleted();
            FindObjectOfType<SpawnCustomer>()?.SpawnCustomerAsset(true,breadSO);
        }
    }
}
