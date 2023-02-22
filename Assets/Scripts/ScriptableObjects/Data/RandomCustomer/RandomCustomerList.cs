using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "RandomCustomerList", menuName = "Data/RandomCustomerList")]
public class RandomCustomerList : ScriptableObject
{
    [SerializeField] private List<AssetReference> randomCustomer;

    public AssetReference RandomCustomer() {
        return randomCustomer[Random.Range(0, randomCustomer.Count)];
    }
}
