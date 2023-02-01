using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class DebugCamera : MonoBehaviour {

    [SerializeField] private GameObject CameraPerspective;
    [SerializeField] private GameObject CameraOrthographic;
    [SerializeField] private PlayerControllerSO controller;
    private bool orthographicCameraIsActive = false;


    private void OnDisable() {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.Disable();
    }

    // Start is called before the first frame update
    void Start() {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.Enable();
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.performed += SwitchCameraFunction;
        CameraOrthographic.SetActive(false);
        CameraPerspective.SetActive(true);
        CameraOrthographic.GetComponent<CinemachineFreeLook>().m_Lens.NearClipPlane = -5;

    }

    private void OnDestroy() {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.performed -= SwitchCameraFunction;
    }


    public void SwitchCameraFunction(InputAction.CallbackContext context) {
        CameraOrthographic.GetComponent<CinemachineFreeLook>().m_Lens.NearClipPlane = -10f;
        if (orthographicCameraIsActive) {
            print("Camera Persp");
            CameraOrthographic.SetActive(false);
            CameraPerspective.SetActive(true);
            orthographicCameraIsActive = false;
        }
        else {
            print("Camera Ortho");
            CameraOrthographic.SetActive(true);
            CameraPerspective.SetActive(false);
            orthographicCameraIsActive = true;
        }
    }

}
