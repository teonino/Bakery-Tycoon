using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    [SerializeField] private PlayerControllerSO controller;
    [SerializeField] private Animator CinemachineAnimator;
    private bool perspectiveCameraIsUsed = true;
    [SerializeField] CinemachineVirtualCamera perspectiveCamera;
    [SerializeField] CinemachineVirtualCamera orthoCamera;

    private void Awake()
    {
        CinemachineAnimator = GetComponent<Animator>();
    }

    public void SwitchState(InputAction.CallbackContext context)
    {
        print("switchState");
        if(perspectiveCameraIsUsed)
        {
            CinemachineAnimator.Play("OrthographicCamera");
        }
        else
        {
            CinemachineAnimator.Play("PerspectiveCamera");
        }
        perspectiveCameraIsUsed = !perspectiveCameraIsUsed;
    }

    private void SwitchPrioriy()
    {
        if(perspectiveCameraIsUsed)
        {
            perspectiveCamera.Priority = 0;
            orthoCamera.Priority = 1;
        }
        else
        {
            perspectiveCamera.Priority = 1;
            orthoCamera.Priority = 0;
        }
        perspectiveCameraIsUsed = !perspectiveCameraIsUsed;
    }

}
