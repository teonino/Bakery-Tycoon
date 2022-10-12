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

    public int nbCustomer = 0;
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
        //Spawn a customer
        if (enableSpawn && nbCustomer <= shelves.Count) {
            customerAsset.InstantiateAsync(transform);
            nbCustomer++;
        }

        //Start SpawnDelay again
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }
}
