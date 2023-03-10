using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour {
    [SerializeField] private bool lookCamera = true;
    private GameObject camera;

    private void Awake()
    {
        ChangeCamera();
    }

    // Update is called once per frame
    void Update() {
        if (lookCamera)
            transform.LookAt(camera.transform);
    }

    public void ChangeCamera()
    {
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
