using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {
    [SerializeField] private float speed = 5;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource footstepSource;

    private Rigidbody rb;
    private Camera cam;
    private void Awake() {
        rb = GetComponent<Rigidbody>(); 
        cam = Camera.main;
    }

    public void Move(Vector2 movement)
    {
        Vector3 velocity = new Vector3(movement.x, 0, movement.y);
        velocity = cam.transform.TransformDirection(velocity);
        velocity.y = 0;
        rb.velocity = velocity.normalized * speed;
        rb.rotation = Quaternion.LookRotation(velocity);
    }

    private Vector3 MultipleVector3(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x * v2.x, v1.y *v2.y, v1.z * v2.z);
    }
}
