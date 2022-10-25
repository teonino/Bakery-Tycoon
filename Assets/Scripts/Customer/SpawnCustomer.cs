using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCustomer : MonoBehaviour {
    [Header("Spawn attributes")]
    [SerializeField] private bool enableSpawn;
    [SerializeField] private int minDelaySpawn;
    [SerializeField] private int maxDelaySpawn;
    [SerializeField] private int nbCustomer = 0;
    [SerializeField] private int nbCustomerMax = 5;
    [SerializeField] private AssetReference customerAsset;
    [SerializeField] private AssetReference regularCustomerAsset;

    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }

    private IEnumerator SpawnDelay(int time) {
        yield return new WaitForSeconds(time);
        InstantiateCustomer();
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }

    private void InstantiateCustomer() {
        //Spawn a customer
        if (enableSpawn && nbCustomer < nbCustomerMax && gameManager.GetDayTime() == DayTime.Day) {
            if (Random.Range(1, 10) == 1)
                regularCustomerAsset.InstantiateAsync(transform);
            else
                customerAsset.InstantiateAsync(transform);
            nbCustomer++;
        }
    }

    public void RemoveCustomer() => nbCustomer--;
}
