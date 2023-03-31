using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour {
    [SerializeField] private bool lookCamera = true;
    private GameObject cameraObject;

    private void Awake() {
        ChangeCamera();
    }

    // Update is called once per frame
    void Update() {
        if (lookCamera)
            if (cameraObject)
                transform.LookAt(cameraObject.transform);
            else
                cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void ChangeCamera() {
        StartCoroutine(wait());
    }

    private IEnumerator wait() {
        yield return new WaitForEndOfFrame();
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
