using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerBonus", menuName = "Data/CustomerBonus")]
public class CustomerBonusSO : ScriptableObject
{
    [Tooltip("0 is default price")]
    [SerializeField] private float multiplierBonus = 0;

    public float GetMultiplier() => multiplierBonus;
}
