using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour {
    [SerializeField] private bool lookCamera = true;

    // Update is called once per frame
    void Update() {
        if (lookCamera && Camera.main && Camera.main.tag == "MainCamera")
            transform.LookAt(Camera.main.transform);
    }
}
