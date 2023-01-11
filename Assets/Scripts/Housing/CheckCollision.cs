using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
    public int layer;
    public CheckCollisionManager manager;
    public int nbCollision = 0;

    private void OnCollisionEnter(Collision collision) {
        if (layer == LayerMask.NameToLayer("CustomizableWall")) {
            if (collision.gameObject.layer == layer || collision.gameObject.tag == "Floor") {
                nbCollision++;
                manager.CheckNbCollision();
            }
        }
        else {
            if (collision.gameObject.layer == layer || collision.gameObject.tag == "Wall") {
                nbCollision++;
                manager.CheckNbCollision();
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (layer == LayerMask.NameToLayer("CustomizableWall")) {
            if (collision.gameObject.layer == layer || collision.gameObject.tag == "Floor") {
                nbCollision--;
                manager.CheckNbCollision();
            }
        }
        else {
            if (collision.gameObject.layer == layer || collision.gameObject.tag == "Wall") {
                nbCollision--;
                manager.CheckNbCollision();
            }
        }
    }
}
