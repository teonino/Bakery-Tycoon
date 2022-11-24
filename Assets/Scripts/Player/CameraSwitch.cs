using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private GameObject CameraMainRoomPosition;
    [SerializeField] private GameObject CameraStorageRoomPosition;
    [SerializeField] private Collider TriggerMainRoom;
    private float lerpRange = 0.0f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //cam.transform.position = new Vector3.Lerp(CameraMainRoomPosition.transform.position, CameraStorageRoomPosition.transform.position, lerpRange);
        }
        print("Storage");
    }

    private void OnTriggerExit(Collider other)
    {
        print("MainRoom");
        
    }



}
