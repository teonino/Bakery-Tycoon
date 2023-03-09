using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCamera : MonoBehaviour
{
    [SerializeField] private GameObject perspectiveCamera;
    [SerializeField] private GameObject orthographicCamera;
    internal bool usePerspectiveCamera = true;

    public Action ChangeLookCamera;
    private LookCamera[] lookCameras;

    private void Awake()
    {
        lookCameras = FindObjectsOfType<LookCamera>();

        foreach (LookCamera lookCamera in lookCameras)
            ChangeLookCamera += lookCamera.ChangeCamera;
    }

    public void ChangeCamera()
    {
        if(perspectiveCamera != null && orthographicCamera != null)
        {
            if(usePerspectiveCamera)
            {
                perspectiveCamera.SetActive(false);
                orthographicCamera.SetActive(true);
                usePerspectiveCamera = false;
            }
            else
            {
                orthographicCamera.SetActive(false);
                perspectiveCamera.SetActive(true);
                usePerspectiveCamera = true;
            }

            ChangeLookCamera?.Invoke();
        }
    }

    private void OnDestroy()
    {
        foreach (LookCamera lookCamera in lookCameras)
            ChangeLookCamera -= lookCamera.ChangeCamera;
    }
}
