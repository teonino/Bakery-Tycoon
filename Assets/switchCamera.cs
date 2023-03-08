using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCamera : MonoBehaviour
{
    [SerializeField] private GameObject perspectiveCamera;
    [SerializeField] private GameObject orthographicCamera;
    internal bool usePerspectiveCamera = true;

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
        }
    }
}
