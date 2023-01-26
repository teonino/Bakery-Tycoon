using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomersSO", menuName = "Data/Customers")]
public class CustomersSO : ScriptableObject
{
    [SerializeField] private int nbRandomCustomer;
    [SerializeField] private int nbRegularCustomer;
    [SerializeField] private Vector2 delaySpawn;

    public int GetNbRandomCustomer() => nbRandomCustomer;
    public int GetNbRegularCustomer() => nbRegularCustomer;
    public Vector2 GetDelaySpawn() => delaySpawn;
}
