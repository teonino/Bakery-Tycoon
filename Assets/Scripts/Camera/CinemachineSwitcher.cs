using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    [SerializeField] private PlayerControllerSO controller;
    [SerializeField] private Animator CinemachineAnimator;
    [SerializeField] private CinemachineVirtualCamera perspectiveCamera;
    [SerializeField] private CinemachineVirtualCamera OrthographicCamera;
    private bool perspectiveCameraIsUsed = true;

    private void Awake()
    {
        CinemachineAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.Enable();
    }

    private void OnDisable()
    {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.Disable();
    }

    private void Start()
    {
        controller.GetPlayerController().playerInput.Debug.SwitchCamera.performed += _ => SwitchState();
    }

    private void SwitchState()
    {
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
            OrthographicCamera.Priority = 1;
        }
        else
        {
            perspectiveCamera.Priority = 1;
            OrthographicCamera.Priority = 0;
        }
        perspectiveCameraIsUsed = !perspectiveCameraIsUsed;
    }

}
