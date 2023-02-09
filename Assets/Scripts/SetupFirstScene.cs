using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupFirstScene : MonoBehaviour
{
    [SerializeField] BuildingMode buildingMode;
    [SerializeField] Camera mainCamera;
    private void Start()
    {
        print("oui");
        buildingMode.Effect();
        mainCamera.enabled = false;
    }

}
