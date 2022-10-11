using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCustomer : MonoBehaviour {
    [Header("Spawn attributes")]
    [SerializeField] private bool enableSpawn;
    [SerializeField] private int minDelaySpawn = 3;
    [SerializeField] private int maxDelaySpawn = 5;
    [SerializeField] private AssetReference customerAsset;

    private List<Shelf> shelves;
    private bool canSpawn = false;

    void Start() {
        shelves = new List<Shelf>(FindObjectsOfType<Shelf>());
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }

    private IEnumerator SpawnDelay(int time) {
        yield return new WaitForSeconds(time);
        Spawn();
    }

    private void Spawn() {
        //Check if a shelf is empty
        foreach (Shelf shelf in shelves) {
            if (!shelf.occupied)
                canSpawn = true;
        }

        //Spawn a customer
        if (canSpawn && enableSpawn)
            customerAsset.InstantiateAsync(transform);

        //Reset canSpawn 
        canSpawn = false;

        //Start SpawnDelay again
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }
}
