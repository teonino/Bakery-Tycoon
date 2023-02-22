using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> PathPoint;
    [SerializeField] private List<Material> CarMaterial;
    [SerializeField] private List<Material> TruckMaterial;
    [SerializeField] private int carSpawnPercentage;
    [SerializeField] private List<GameObject> carAndTruckModel;
    [SerializeField] private List<GameObject> spawnPoint;
    private GameObject Vehicule;
    private List<GameObject> Truckspawned;
    private List<GameObject> Carspawned;


    private void OnEnable()
    {
        StartCoroutine(spawnVehicule());
    }

    private IEnumerator spawnVehicule()
    {
        int randomVehicule = Random.Range(0, 100);
        if(randomVehicule <= carSpawnPercentage)
        {
            Vehicule = carAndTruckModel[0];

            int randomColor = Random.Range(0, CarMaterial.Count);
            Vehicule.GetComponentInChildren<MeshRenderer>().material = CarMaterial[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            Carspawned.Add(Instantiate(Vehicule, spawnPoint[randomSpawnPoint].transform));
        }
        else
        {
            Vehicule = carAndTruckModel[1];

            int randomColor = Random.Range(0, TruckMaterial.Count);
            Vehicule.GetComponentInChildren<MeshRenderer>().material = TruckMaterial[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            Truckspawned.Add(Instantiate(Vehicule, spawnPoint[randomSpawnPoint].transform));
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(spawnVehicule());
    }

    private void destroyAllSpawnedVéhicule()
    {
        for(int i = 0; i < Carspawned.Count; i++)
        {
            Destroy(Carspawned[i]);
            Carspawned.Clear();
        }
        for (int i = 0; i < Truckspawned.Count; i++)
        {
            Destroy(Truckspawned[i]);
            Truckspawned.Clear();
        }
    }

    private void destroyLastVéhicule()
    {
        Destroy(Carspawned[Carspawned.Count]);
        Carspawned.Remove(Carspawned[Carspawned.Count]);
        Destroy(Truckspawned[Truckspawned.Count]);
        Truckspawned.Remove(Truckspawned[Truckspawned.Count]);
    }

}
