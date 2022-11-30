using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    private float playerLocalisation = 0;
    [SerializeField] private GameObject MainRoomSocket;
    [SerializeField] private GameObject StorageCamSocket;
    private float LerpTime = 0.5f;

    private void Update()
    {
        if(playerLocalisation == 0)
        {
            print("Default");
            MainRoomSocket.transform.position = MainRoomSocket.transform.position;
        }
        else if (playerLocalisation == 1)
        {
            print("MainRoom");
            MainRoomSocket.transform.position = Vector3.Slerp(StorageCamSocket.transform.position, MainRoomSocket.transform.position, LerpTime * Time.deltaTime);


        }
        else if (playerLocalisation == 2)
        {
            print("Storage");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerLocalisation = 1;
        MainRoomSocket.transform.position = Vector3.Slerp(MainRoomSocket.transform.position, StorageCamSocket.transform.position, LerpTime * Time.deltaTime);

    }

    private void OnTriggerExit(Collider other)
    {
        playerLocalisation = 2;

    }


}
