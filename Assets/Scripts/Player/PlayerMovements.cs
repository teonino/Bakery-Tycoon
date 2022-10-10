using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {
    [SerializeField] private float speed = 5;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>(); 
    }

    public void Move(Vector2 movement) {
        Vector3 velocity = new Vector3(movement.x, 0, movement.y);

        rb.velocity = velocity.normalized * speed;
        rb.rotation = Quaternion.LookRotation(velocity);
    }
}
