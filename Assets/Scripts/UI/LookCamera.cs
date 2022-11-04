using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour {
    [SerializeField] private bool lookCamera = true;
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start() {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update() {
        if (lookCamera)
            transform.LookAt(cameraTransform);
    }
}
