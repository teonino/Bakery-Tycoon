using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderTypeQuest", menuName = "Quest/OrderTypeQuest")]
public class OrderTypeQuest : Quest {
    [Header("Order Type Quest Parameters")]
    [SerializeField] private DeliveryType deliveryType;

    public void CheckDeliveryType(DeliveryType deliveryType) {
        if (isActive && this.deliveryType == deliveryType)
            OnCompleted();
    }
}
