using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour {
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
    }

    public bool switchingCamera;

    private IEnumerator EnableCinemachine(GameObject dest) {
        cinemachine.enabled = switchingCamera =true;
        yield return null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < wallFadeScriptMainRoom.Count; i++) {
                wallFadeScriptMainRoom[i].thisRoomIsActive = false;
            }

            for (int i = 0; i < wallFadeScriptStorage.Count; i++) {
                wallFadeScriptStorage[i].thisRoomIsActive = true;
                wallFadeScriptMainRoom[0].DisableWall();
            }

            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < wallFadeScriptMainRoom.Count; i++) {
                wallFadeScriptMainRoom[i].thisRoomIsActive = true;
            }

            for (int i = 0; i < wallFadeScriptStorage.Count; i++) {
                wallFadeScriptStorage[i].thisRoomIsActive = false;
                wallFadeScriptMainRoom[0].EnableWall();
            }

            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }
}
