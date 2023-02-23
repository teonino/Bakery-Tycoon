using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OutdoorManager : MonoBehaviour
{
    public PlayerControllerSO playercontroller;
    [SerializeField] private List<GameObject> PathPoint;
    [SerializeField] private List<Material> CarMaterial;
    [SerializeField] private List<Material> TruckMaterial;
    [SerializeField] private int carSpawnPercentage;
    [SerializeField] private List<GameObject> carAndTruckModel;
    [SerializeField] private List<GameObject> spawnPoint;
    private GameObject Vehicule;
    public List<GameObject> Truckspawned = new List<GameObject>();
    public List<GameObject> Carspawned = new List<GameObject>();

    private void OnEnable()
    {
        StartCoroutine(spawnVehicule());
    }


    private IEnumerator spawnVehicule()
    {
        
        int randomVehicule = Random.Range(0, 100);
        if (randomVehicule <= carSpawnPercentage)
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

            print("Vehicule: + " + Vehicule);
            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            print(randomSpawnPoint);
            Truckspawned.Add(Instantiate(Vehicule, spawnPoint[randomSpawnPoint].transform));
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(deletelastveihucleCoroutine());
        StartCoroutine(spawnVehicule());
        destroyLastVehicule();
    }

    IEnumerator deletelastveihucleCoroutine()
    {
        yield return new WaitForSeconds(1f);
        destroyLastVehicule();
    }

    private void destroyAllSpawnedVehicule()
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

    private void destroyLastVehicule()
    {
        //Destroy(Carspawned[Carspawned.Count-1]);
        //Carspawned.Remove(Carspawned[Carspawned.Count-1]);
        Destroy(Truckspawned[Truckspawned.Count-1]);
        Truckspawned.Remove(Truckspawned[Truckspawned.Count-1]);
    }

}
