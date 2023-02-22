using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionManager : MonoBehaviour {
    public Material collidingMaterial;
    public int layer;
    private List<Material> initialMaterials;
    private List<GameObject> furnitureColliderList;
    private List<MeshRenderer> furnitureMeshRendererList;

    void Start() {
        furnitureColliderList = new List<GameObject>();
        furnitureMeshRendererList = new List<MeshRenderer>();
        initialMaterials = new List<Material>();

        Collider collider;

        if (gameObject.TryGetComponent(out collider)) {
            CheckCollision component = gameObject.AddComponent<CheckCollision>();
            component.manager = this;
            component.layer = layer;
            furnitureColliderList.Add(gameObject);
        }

        FetchAllCollider(transform);

        //foreach (Transform t in transform) {
        //    if (t.gameObject.TryGetComponent(out collider)) {
        //        CheckCollision childComponent = t.gameObject.AddComponent<CheckCollision>();
        //        childComponent.manager = this;
        //        childComponent.layer = layer;
        //        furnitureList.Add(t.gameObject);
        //    }
        //}
        if (gameObject.TryGetComponent(out collider)) {
            if (transform.TryGetComponent(out MeshRenderer renderer)) {
                furnitureMeshRendererList.Add(renderer);
                initialMaterials.Add(renderer.material);
            }
        }

        FetchAllMaterials(transform);

        //foreach (GameObject go in transform) {
        //    if (go.TryGetComponent(out MeshRenderer renderer))
        //        initialMaterials.Add(renderer.material);
        //}
    }

    private void FetchAllCollider(Transform parent) {
        foreach (Transform transform in parent) {
            Collider collider = transform.GetComponent<Collider>();
            if (collider) {
                CheckCollision childComponent = transform.gameObject.AddComponent<CheckCollision>();
                childComponent.manager = this;
                childComponent.layer = layer;
                furnitureColliderList.Add(transform.gameObject);
            }

            if (transform.childCount > 0)
                FetchAllCollider(transform);
        }
    }

    private void FetchAllMaterials(Transform parent) {
        foreach (Transform transform in parent) {
            if (transform.TryGetComponent(out MeshRenderer renderer)) {
                furnitureMeshRendererList.Add(renderer);
                initialMaterials.Add(renderer.material);
            }
            if (transform.childCount > 0)
                FetchAllMaterials(transform);
        }
    }

    public void CollisionMaterial() {
        foreach (MeshRenderer renderer in furnitureMeshRendererList)
            renderer.material = collidingMaterial;
    }

    public void InitialMaterial() {
        for (int i = 0; i < furnitureMeshRendererList.Count; i++) {
            furnitureMeshRendererList[i].material = initialMaterials[i];
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
        foreach (GameObject go in furnitureColliderList)
            total += go.GetComponent<CheckCollision>().nbCollision;

        return total;
    }

    private void OnDestroy() {
        foreach (GameObject go in furnitureColliderList)
            Destroy(go.GetComponent<CheckCollision>());

        for (int i = 0; i < furnitureMeshRendererList.Count; i++) {
            furnitureMeshRendererList[i].material = initialMaterials[i];
        }
    }
}
