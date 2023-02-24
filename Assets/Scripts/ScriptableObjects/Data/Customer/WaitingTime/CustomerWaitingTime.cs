using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerWaitingTime", menuName = "Data/CustomerWaitingTime")]
public class CustomerWaitingTime : ScriptableObject
{
    [SerializeField] private int waitingTime = 5;

    public int GetWaitingTime() => waitingTime;
}
