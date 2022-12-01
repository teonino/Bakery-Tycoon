using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    private float playerLocalisation = 0;
    [SerializeField] private GameObject MainRoomSocket;
    [SerializeField] private GameObject StorageCamSocket;
    [SerializeField] private GameObject CurrentCamPosition;
    [SerializeField] private float LerpTime;

    private void Start()
    {
        CurrentCamPosition.transform.position = MainRoomSocket.transform.position;
    }

    private void Update()
    {
        if(playerLocalisation == 1)
        {
            CurrentCamPosition.transform.position = Vector3.Slerp(CurrentCamPosition.transform.position, StorageCamSocket.transform.position, LerpTime * Time.deltaTime);

        }
        else if(playerLocalisation == 2)
        {
            CurrentCamPosition.transform.position = Vector3.Slerp(CurrentCamPosition.transform.position, MainRoomSocket.transform.position, LerpTime * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerLocalisation = 1;

    }

    private void OnTriggerExit(Collider other)
    {
        playerLocalisation = 2;
    }


}
