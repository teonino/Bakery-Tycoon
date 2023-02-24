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
    [SerializeField] private List<Drive> carAndTruckModels;
    [SerializeField] internal List<GameObject> spawnPoint;
    [SerializeField] internal GameObject HorizontalPathPointParent;
    [SerializeField] internal GameObject VerticalPathPointParent;
    [SerializeField] private float carSpawnFrequency = 4f;
    //[SerializeField] private Drive carScript;
    private Drive vehiculeBeingSpawned;
    private int nbVehiculeSpawned = 0;

    private void OnEnable()
    {
        StartCoroutine(spawnVehicule());
    }


    private IEnumerator spawnVehicule()
    {
        int randomVehicule = Random.Range(0, 100);
        Drive Vehicle;
        List<Transform> path = new List<Transform>();

        if (randomVehicule <= carSpawnPercentage)
        {
            if(randomVehicule <= carSpawnPercentage/2)
            {
                Vehicle = carAndTruckModels[0];

                int randomColor = Random.Range(0, Car1Material.Count);
                Vehicle.vehicleMesh.material = Car1Material[randomColor];
            }
            else
            {
                Vehicle = carAndTruckModels[1];

                int randomColor = Random.Range(0, Car2Material.Count);
                Vehicle.vehicleMesh.material = Car2Material[randomColor];
            }

        }
        else
        {
            Vehicle = carAndTruckModels[2];

            int randomColor = Random.Range(0, TruckMaterial.Count);
            Vehicle.vehicleMesh.material = TruckMaterial[randomColor];
        }

        //Instantiate + setting the path
        vehiculeBeingSpawned = Instantiate<Drive>(Vehicle, this.transform);
        nbVehiculeSpawned++;
        if (nbVehiculeSpawned % 2 == 0)
        {

            for (int i = 0; i < HorizontalPathPointParent.transform.childCount; i++)
            {
                path.Add(HorizontalPathPointParent.transform.GetChild(i).transform);
            }
        }
        else
        {
            for (int i = 0; i < VerticalPathPointParent.transform.childCount; i++)
            {
                path.Add(VerticalPathPointParent.transform.GetChild(i).transform);
            }
        }

        vehiculeBeingSpawned.SetPath(path);


        yield return new WaitForSeconds(carSpawnFrequency);
        StartCoroutine(spawnVehicule());
    }

}
