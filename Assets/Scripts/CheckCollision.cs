using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
    public int nbObjectInCollision = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Floor") {
            nbObjectInCollision++;
            print("Trigger : " +nbObjectInCollision + " / " + other.name);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag != "Floor") {
            nbObjectInCollision--;
            print("Trigger : " + nbObjectInCollision + " / " + other.name);
        }
    }

    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.collider.tag != "Floor") {
    //        nbObjectInCollision--;
    //        print("Collision : " + nbObjectInCollision + " / " + collision.collider.name);
    //    }
    //}
    //private void OnCollisionExit(Collision collision) {
    //    if (collision.collider.tag != "Floor") {
    //        nbObjectInCollision++;
    //        print("Collision : " + nbObjectInCollision + " / " + collision.collider.name);
    //    }
    //}
}
