using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionManager : MonoBehaviour {
    public Material collidingMaterial;
    private List<Material> initialMaterials;
    private List<GameObject> furnitureList;

    void Start() {
        furnitureList = new List<GameObject>();
        initialMaterials = new List<Material>();

        CheckCollision component = gameObject.AddComponent<CheckCollision>();
        component.manager = this;
        furnitureList.Add(gameObject);
        foreach (Transform t in transform) {
            if (t.gameObject.GetComponent<MeshRenderer>()) {
                CheckCollision childComponent = t.gameObject.AddComponent<CheckCollision>();
                childComponent.manager = this;
                furnitureList.Add(t.gameObject);
            }
        }

        foreach (GameObject go in furnitureList) {
            if(go.GetComponent<MeshRenderer>())
                initialMaterials.Add(go.GetComponent<MeshRenderer>().material);
        }
    }

    public void CollisionMaterial() {
        foreach (GameObject go in furnitureList) {
            go.GetComponent<MeshRenderer>().material = collidingMaterial;
        }
    }

    public void InitialMaterial() {
        for (int i = 0; i < furnitureList.Count; i++) {
            furnitureList[i].GetComponent<MeshRenderer>().material = 
                initialMaterials[i];
        }
    }

    public void CheckNbCollision() {
        if (GetNbCollision() > 0)
            CollisionMaterial();
        else
            InitialMaterial();
    }

    public int GetNbCollision() {
        int total = 0;
        foreach(GameObject go in furnitureList) {
            total += go.GetComponent<CheckCollision>().nbCollision;
        }
        return total;
    }

    private void OnDestroy() {
        foreach (GameObject go in furnitureList) {
            Destroy(go.GetComponent<CheckCollision>());
        }
    }
}
