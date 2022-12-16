using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour {
    [SerializeField] private GameObject MainRoomSocket;
    [SerializeField] private GameObject StorageCamSocket;
    [SerializeField] private GameObject CurrentCamPosition;
    [SerializeField] private float LerpTime;
    [SerializeField] private List<WallFade> wallFadeScriptMainRoom;
    [SerializeField] private List<WallFade> wallFadeScriptStorage;
    private CinemachineFreeLook cinemachine;
    private Coroutine coroutine;

    private void Awake() {
        for (int i = 0; i < wallFadeScriptMainRoom.Count; i++) {
            wallFadeScriptMainRoom[i].thisRoomIsActive = true;
        }
    }

    private void Start() {
        cinemachine = FindObjectOfType<CinemachineFreeLook>();
        CurrentCamPosition.transform.position = MainRoomSocket.transform.position;
    }

    public bool switchingCamera;

    private IEnumerator EnableCinemachine(GameObject dest) {
        cinemachine.enabled = switchingCamera =true;
        while (Vector3.Distance(CurrentCamPosition.transform.position, dest.transform.position) > 0.2f) {
            CurrentCamPosition.transform.position = Vector3.Slerp(CurrentCamPosition.transform.position, dest.transform.position, LerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        cinemachine.enabled = switchingCamera = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < wallFadeScriptMainRoom.Count; i++) {
                wallFadeScriptMainRoom[i].thisRoomIsActive = false;
            }

            for (int i = 0; i < wallFadeScriptStorage.Count; i++) {
                wallFadeScriptStorage[i].thisRoomIsActive = true;
            }

            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(EnableCinemachine(StorageCamSocket));
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < wallFadeScriptMainRoom.Count; i++) {
                wallFadeScriptMainRoom[i].thisRoomIsActive = true;
            }

            for (int i = 0; i < wallFadeScriptStorage.Count; i++) {
                wallFadeScriptStorage[i].thisRoomIsActive = false;
            }

            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(EnableCinemachine(MainRoomSocket));
        }
    }
}
