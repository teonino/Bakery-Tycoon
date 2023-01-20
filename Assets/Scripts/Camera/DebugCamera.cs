using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class DebugCamera : MonoBehaviour
{

    [SerializeField] private GameObject CameraPerspective;
    [SerializeField] private GameObject CameraOrthographic;
    [SerializeField] private PlayerControllerSO controller;
    private bool orthographicCameraIsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.performed += SwitchCameraFunction;
        CameraOrthographic.SetActive(false);
        CameraPerspective.SetActive(true);

    }

    private void OnEnable()
    {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.Enable();
    }

    public void SwitchCameraFunction(InputAction.CallbackContext context)
    {
        CameraOrthographic.GetComponent<CinemachineFreeLook>().m_Lens.NearClipPlane = -5;
        if(orthographicCameraIsActive)
        {
            print("Camera Persp");
            CameraOrthographic.SetActive(false);
            CameraPerspective.SetActive(true);
            orthographicCameraIsActive = false;
        }
        else
        {
            print("Camera Ortho");
            
            CameraOrthographic.SetActive(true);
            CameraPerspective.SetActive(false);
            orthographicCameraIsActive = true;
        }
    }

}
