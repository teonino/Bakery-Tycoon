using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
    public int nbObjectInCollision = 0;
    public Material collidingMaterial;
    public Material initialMaterial;
    private Mesh mesh;

    private void Awake() {
        initialMaterial = GetComponent<MeshRenderer>().material;
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag != "Floor") {
            
            //Vector3[] vertices = mesh.vertices;
            //for (int i = 0; i < vertices.Length; i++) {
            //    if (collision.collider.bounds.Contains(vertices[i]))
            //        print("yes");
            //}

            nbObjectInCollision++;
            GetComponent<MeshRenderer>().material = collidingMaterial;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag != "Floor") {
            nbObjectInCollision--;
            if (nbObjectInCollision == 0)
                GetComponent<MeshRenderer>().material = initialMaterial;
        }
    }
}
