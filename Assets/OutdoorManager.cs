using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OutdoorManager : MonoBehaviour
{
    public PlayerControllerSO playercontroller;
    [SerializeField] private List<Material> Car1Material;
    [SerializeField] private List<Material> Car2Material;
    [SerializeField] private List<Material> TruckMaterial;
    [SerializeField] private int carSpawnPercentage;
    [SerializeField] private List<GameObject> carAndTruckModel;
    [SerializeField] internal List<GameObject> spawnPoint;
    private GameObject Vehicule;
    private List<GameObject> Truckspawned = new List<GameObject>();
    private List<GameObject> Carspawned = new List<GameObject>();
    [SerializeField] internal GameObject HorizontalPathPointParent;
    [SerializeField] internal GameObject VerticalPathPointParent;

    private void OnEnable()
    {
        StartCoroutine(spawnVehicule());
    }


    private IEnumerator spawnVehicule()
    {
        yield return new WaitForSeconds(3);
        int randomVehicule = Random.Range(0, 100);
        if (randomVehicule <= carSpawnPercentage / 2)
        {
            Vehicule = carAndTruckModel[0];

            int randomColor = Random.Range(0, Car2Material.Count);
            Vehicule.GetComponent<MeshRenderer>().material = Car1Material[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            //destroyLastVehicule();
            Carspawned.Add(Instantiate(Vehicule, spawnPoint[1].transform));
            Carspawned[Carspawned.Count - 1].transform.position = spawnPoint[1].transform.position;
            Carspawned[Carspawned.Count - 1].transform.rotation = spawnPoint[1].transform.rotation;
        }
        else if (randomVehicule >= (carSpawnPercentage / 2) + 1 && randomVehicule < carSpawnPercentage + 1)
        {

            Vehicule = carAndTruckModel[1];

            int randomColor = Random.Range(0, Car2Material.Count);
            Vehicule.GetComponent<MeshRenderer>().material = Car2Material[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            //destroyLastVehicule();
            Carspawned.Add(Instantiate(Vehicule, spawnPoint[1].transform));
            Carspawned[Carspawned.Count - 1].transform.position = spawnPoint[1].transform.position;
            Carspawned[Carspawned.Count - 1].transform.rotation = spawnPoint[1].transform.rotation;
        }
        else if (randomVehicule > carSpawnPercentage)
        {
            Vehicule = carAndTruckModel[2];

            int randomColor = Random.Range(0, TruckMaterial.Count);
            Vehicule.GetComponentInChildren<MeshRenderer>().material = TruckMaterial[randomColor];

            int randomSpawnPoint = Random.Range(0, spawnPoint.Count);
            //destroyLastVehicule();
            Truckspawned.Add(Instantiate(Vehicule, spawnPoint[1].transform));
            Truckspawned[Truckspawned.Count - 1].transform.position = spawnPoint[1].transform.position;
            Truckspawned[Truckspawned.Count - 1].transform.rotation = spawnPoint[1].transform.rotation;
        }
        yield return new WaitForSeconds(90f);
        StartCoroutine(spawnVehicule());
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

    public void destroyLastVehicule()
    {
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
