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
    [SerializeField] private List<WallFade> wallFadeScriptMainRoom;
    [SerializeField] private List<WallFade> wallFadeScriptStorage;

    private void Awake()
    {
        for (int i = 0; i < wallFadeScriptMainRoom.Count; i++)
        {
            wallFadeScriptMainRoom[i].thisRoomIsActive = true;
        }
    }

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
        if (other.gameObject.tag == "Player")
        {
            playerLocalisation = 1;
            for (int i = 0; i < wallFadeScriptMainRoom.Count; i++)
            {
                wallFadeScriptMainRoom[i].thisRoomIsActive = false;
            }

            for (int i = 0; i < wallFadeScriptStorage.Count; i++)
            {
                wallFadeScriptStorage[i].thisRoomIsActive = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerLocalisation = 2;
            for (int i = 0; i < wallFadeScriptMainRoom.Count; i++)
            {
                wallFadeScriptMainRoom[i].thisRoomIsActive = true;
            }

            for (int i = 0; i < wallFadeScriptStorage.Count; i++)
            {
                wallFadeScriptStorage[i].thisRoomIsActive = false;
            }
        }
    }


}
