using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class BuildingMode : Interactable {
    [Header("References")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject buildingCamera;
    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private LayerMask putDownLayer;
    [SerializeField] private Material collidingMaterial;

    [Header("Global Parameters")]
    [SerializeField] private float snapValue;

    [Header("Gamepad Parameters")]
    [SerializeField] private AssetReference cursor;
    [SerializeField] private int cursorSpeed = 1;

    private LayerMask currentRaycastlayer;
    private LayerMask intitialGoLayer;
    private GameObject cursorObject;
    private GameObject selectedGo;
    private bool inBuildingMode = false;

    private void Start() {
        currentRaycastlayer = pickUpLayer;
        playerController.playerInput.Building.Quit.performed += Quit;
        playerController.playerInput.Building.Select.performed += Select;
    }

    public override void Effect() {
        if (gameManager.dayTime == DayTime.Morning) {
            playerController.DisableInput();
            playerController.playerInput.Building.Enable();

            if (gameManager.IsGamepad()) {
                cursor.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
                    cursorObject = go.Result;
                };
            }
            inBuildingMode = true;
            mainCamera.SetActive(false);
            buildingCamera.SetActive(true);
        }
    }

    public void Quit(CallbackContext context) {
        if (context.performed && !selectedGo) {
            if (cursorObject) {
                Addressables.ReleaseInstance(cursorObject);
            }
            playerController.playerInput.Building.Disable();
            playerController.EnableInput();
            mainCamera.SetActive(true);
            buildingCamera.SetActive(false);
            inBuildingMode = false;
        }
    }

    public void Select(CallbackContext context) {
        Ray ray;
        if (gameManager.IsGamepad()) {
            ray = Camera.main.ScreenPointToRay(cursorObject.transform.position);
        }
        else
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
            if (!selectedGo) {
                currentRaycastlayer = putDownLayer;
                selectedGo = hit.collider.gameObject;
                intitialGoLayer = selectedGo.layer;
                selectedGo.layer = 3;
                selectedGo.AddComponent<CheckCollision>();
                selectedGo.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                selectedGo.GetComponent<CheckCollision>().collidingMaterial = collidingMaterial;

                ChangeColliderSize(true);
            }
            else {
                if (selectedGo.GetComponent<CheckCollision>().nbObjectInCollision == 0) {
                    currentRaycastlayer = pickUpLayer;
                    selectedGo.layer = intitialGoLayer;
                    Destroy(selectedGo.GetComponent<CheckCollision>());
                    Destroy(selectedGo.GetComponent<Rigidbody>());

                    ChangeColliderSize(false);

                    selectedGo = null;
                }
            }
        }
    }

    private void ChangeColliderSize(bool remove) {
        if (selectedGo.TryGetComponent(out BoxCollider boxCol))
            if (remove)
                boxCol.size -= Vector3.one * 0.1f;
            else
                boxCol.size += Vector3.one * 0.1f;
        else if (selectedGo.TryGetComponent(out CapsuleCollider capCol))
            if (remove)
                capCol.radius -= 0.1f;
            else
                capCol.radius += 0.1f;
        else if (selectedGo.TryGetComponent(out SphereCollider spheCol))
            if (remove)
                spheCol.radius -= 0.1f;
            else
                spheCol.radius += 0.1f;
    }

    private void FixedUpdate() {
        if (inBuildingMode) {
            if (gameManager.IsGamepad() && cursorObject) {
                cursorObject.transform.Translate(gameManager.GetPlayerController().playerInput.Building.Move.ReadValue<Vector2>() * cursorSpeed);
                SnapGameObject(cursorObject.transform.position);
            }
            else
                SnapGameObject(Mouse.current.position.ReadValue());
        }
    }

    private void SnapGameObject(Vector3 pos) {
        if (selectedGo) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit, Mathf.Infinity, currentRaycastlayer))
                selectedGo.transform.position = new Vector3(
                    RoundToNearestGrid(hit.point.x),
                    selectedGo.transform.localScale.y / 2,
                    RoundToNearestGrid(hit.point.z));
        }
    }

    float RoundToNearestGrid(float pos) => pos - pos % snapValue;
}

