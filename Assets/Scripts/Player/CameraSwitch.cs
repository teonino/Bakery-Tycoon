using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private GameObject CameraMainRoomPosition;
    [SerializeField] private GameObject CameraStorageRoomPosition;
    [SerializeField] private Collider TriggerMainRoom;
    private int CameraIsMoving;
    private float lerpRatio = 0.0f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (CameraIsMoving == 1)
        {
            lerpRatio += 1.25f * Time.deltaTime;
            cam.transform.position = Vector3.Lerp(cam.transform.position, CameraStorageRoomPosition.transform.position, lerpRatio);
        }
        else if (CameraIsMoving == 2)
        {
            lerpRatio += 1.25f * Time.deltaTime;
            cam.transform.position = Vector3.Lerp(cam.transform.position, CameraMainRoomPosition.transform.position, lerpRatio);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CameraIsMoving = 1;
            lerpRatio = 0.0f;
        }
        print("Storage");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CameraIsMoving = 2;
            lerpRatio = 0.0f;
        }
        print("MainRoom");
        
    }



}
