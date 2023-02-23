using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OutdoorManager : MonoBehaviour
{
    public PlayerControllerSO playercontroller;
    [SerializeField] private List<GameObject> PathPoint;
    [SerializeField] private List<Material> Car1Material;
    [SerializeField] private List<Material> Car2Material;
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
        print(randomVehicule);
        if (randomVehicule <= carSpawnPercentage / 2)
        {
            print("voiture 1");
            Vehicule = carAndTruckModel[0];

            int randomColor = Random.Range(0, Car2Material.Count);
            Vehicule.GetComponent<MeshRenderer>().material = Car1Material[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            destroyLastVehicule();
            Carspawned.Add(Instantiate(Vehicule, spawnPoint[randomSpawnPoint].transform));
            Carspawned[Carspawned.Count - 1].transform.position = spawnPoint[randomSpawnPoint].transform.position;
            Carspawned[Carspawned.Count - 1].transform.rotation = spawnPoint[randomSpawnPoint].transform.rotation;
        }
        else if (randomVehicule >= (carSpawnPercentage / 2) + 1 && randomVehicule < carSpawnPercentage + 1)
        {
            print("voiture 2");
            Vehicule = carAndTruckModel[1];

            int randomColor = Random.Range(0, Car2Material.Count);
            Vehicule.GetComponent<MeshRenderer>().material = Car2Material[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            destroyLastVehicule();
            Carspawned.Add(Instantiate(Vehicule, spawnPoint[randomSpawnPoint].transform));
            Carspawned[Carspawned.Count - 1].transform.position = spawnPoint[randomSpawnPoint].transform.position;
            Carspawned[Carspawned.Count - 1].transform.rotation = spawnPoint[randomSpawnPoint].transform.rotation;
        }
        else if (randomVehicule > carSpawnPercentage)
        {
            print("truck");
            Vehicule = carAndTruckModel[2];

            int randomColor = Random.Range(0, TruckMaterial.Count);
            Vehicule.GetComponentInChildren<MeshRenderer>().material = TruckMaterial[randomColor];

            print("Vehicule: + " + Vehicule);
            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            print(randomSpawnPoint);
            destroyLastVehicule();
            Truckspawned.Add(Instantiate(Vehicule, spawnPoint[randomSpawnPoint].transform));
            Truckspawned[Truckspawned.Count - 1].transform.position = spawnPoint[randomSpawnPoint].transform.position;
            Truckspawned[Truckspawned.Count - 1].transform.rotation = spawnPoint[randomSpawnPoint].transform.rotation;
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(spawnVehicule());
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
        if(Truckspawned.Count > 0)
        {
            Destroy(Truckspawned[Truckspawned.Count - 1]);
            Truckspawned.Remove(Truckspawned[Truckspawned.Count - 1]);
        }
        if(Carspawned.Count > 0)
        {
            Destroy(Carspawned[Carspawned.Count -1]);

            Carspawned.Remove(Carspawned[Carspawned.Count - 1]);
        }
    }

}
