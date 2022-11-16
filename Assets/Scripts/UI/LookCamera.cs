using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour {
    [SerializeField] private bool lookCamera = true;

    // Update is called once per frame
    void Update() {
        if (lookCamera)
            transform.LookAt(Camera.main.transform);
    }
}
