using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
    public CheckCollisionManager manager;
    public int nbCollision = 0;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag != "Floor" && collision.gameObject.tag != "Wall") {
            nbCollision++;
            manager.CheckNbCollision();

        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag != "Floor" && collision.gameObject.tag != "Wall") {
            nbCollision--;
            manager.CheckNbCollision();
        }
    }
}
