using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
    public int nbObjectInCollision = 0;
    public Material collidingMaterial;
    public Material initialMaterial;

    private void Awake() {
        initialMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Floor") {
            nbObjectInCollision++;
            print("Trigger : " +nbObjectInCollision + " / " + other.name);
            GetComponent<MeshRenderer>().material = collidingMaterial;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag != "Floor") {
            nbObjectInCollision--;
            print("Trigger : " + nbObjectInCollision + " / " + other.name);
            if(nbObjectInCollision == 0) 
                GetComponent<MeshRenderer>().material = initialMaterial;
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
