using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TruckDeliveryTimeSO", menuName = "Data/TruckDeliveryTime")]
public class TruckDeliveryTime : ScriptableObject
{
    [SerializeField] private float time;

    public float GetTime() => time;
}
