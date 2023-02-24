using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerBonus", menuName = "Data/CustomerBonus")]
public class CustomerBonusSO : ScriptableObject {
    [Tooltip("0 is default price")]
    [SerializeField] private float multiplierMoneyBonus = 0;
    [SerializeField] private float multiplierReputationBonus = 0;

    public float GetMoneyMultiplier() => multiplierMoneyBonus;
    public float GetReputationMultiplier() => multiplierReputationBonus;

    private void SALEMERDER() {
        Product product = null;

        float nb = product.GetPrice() * multiplierMoneyBonus;

        if (nb < 2) {
            Debug.Log("1");
        }
        else if (nb < 5) {
            Debug.Log("2");
        }
        if (nb < 10) {
            Debug.Log("3");
        }
    }
}
