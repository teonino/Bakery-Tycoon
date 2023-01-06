using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class BuildingMode : Interactable {
    [Header("References")]
    [SerializeField] private Material collidingMaterial;
    [SerializeField] private Day day;
    [SerializeField] private Controller controller;
    [SerializeField] private AssetReference cursor;
    [SerializeField] private InterractQuest interractQuest;

    [Header("Global Parameters")]
    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private LayerMask putDownLayer;
    [SerializeField] private float snapValue;

    [Header("Gamepad Parameters")]
    [SerializeField] private int cursorSpeed = 1;

    private GameObject mainCamera;
    private GameObject buildingCamera;
    private LayerMask currentRaycastlayer;
    private LayerMask intitialGoLayer;
    private GameObject cursorObject;
    private GameObject selectedGo;
    private bool inBuildingMode = false;

    private void Start() {
        currentRaycastlayer = pickUpLayer;
        playerControllerSO.GetPlayerController().playerInput.Building.Quit.performed += Quit;
        playerControllerSO.GetPlayerController().playerInput.Building.Select.performed += Select;
        playerControllerSO.GetPlayerController().playerInput.Building.Rotate.performed += RotateGameObject;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        buildingCamera = GameObject.FindGameObjectWithTag("BuildCamera");
        buildingCamera.SetActive(false);
    }

    public override void Effect() {
        if (day.GetDayTime() != DayTime.Day) {
            playerControllerSO.GetPlayerController().DisableInput();
            playerControllerSO.GetPlayerController().playerInput.Building.Enable();

            if (controller.IsGamepad()) {
                cursor.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
                    cursorObject = go.Result;
                };
            }
            inBuildingMode = true;
            mainCamera.SetActive(false);
            buildingCamera.SetActive(true);

            interractQuest?.OnInterract();
        }
    }

    public void Quit(CallbackContext context) {
        if (context.performed && !selectedGo) {
            if (cursorObject) {
                Addressables.ReleaseInstance(cursorObject);
            }
            playerControllerSO.GetPlayerController().playerInput.Building.Disable();
            playerControllerSO.GetPlayerController().EnableInput();
            mainCamera.SetActive(true);
            buildingCamera.SetActive(false);
            inBuildingMode = false;
        }
    }

    public void Select(CallbackContext context) {
        Ray ray;
        if (controller.IsGamepad()) {
            ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(cursorObject.transform.position);
        }
        else
            ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
            if (!selectedGo) {
                currentRaycastlayer = putDownLayer;
                selectedGo = hit.collider.gameObject;
                intitialGoLayer = selectedGo.layer;
                selectedGo.layer = 3;
                selectedGo.AddComponent<CheckCollisionManager>();
                selectedGo.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                selectedGo.GetComponent<CheckCollisionManager>().collidingMaterial = collidingMaterial;

                ChangeColliderSize(true);
            }
            else {
                if (selectedGo.GetComponent<CheckCollisionManager>().GetNbCollision() == 0) {
                    currentRaycastlayer = pickUpLayer;
                    selectedGo.layer = intitialGoLayer;
                    Destroy(selectedGo.GetComponent<CheckCollisionManager>());
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
            if (controller.IsGamepad() && cursorObject) {
                cursorObject.transform.Translate(playerControllerSO.GetPlayerController().playerInput.Building.Move.ReadValue<Vector2>() * cursorSpeed);
                SnapGameObject(cursorObject.transform.position);
            }
            else {
                SnapGameObject(Mouse.current.position.ReadValue());
            }
        }
    }

    private void RotateGameObject(InputAction.CallbackContext ctx) {
        if (selectedGo) {
            if (playerControllerSO.GetPlayerController().playerInput.Building.Rotate.ReadValue<float>() > 0) {
                selectedGo.transform.Rotate(Vector3.up * 90);
            }
            else if (playerControllerSO.GetPlayerController().playerInput.Building.Rotate.ReadValue<float>() < 0) {
                selectedGo.transform.Rotate(Vector3.up * -90);
            }
        }
    }

    private void SnapGameObject(Vector3 pos) {
        if (selectedGo) {
            RaycastHit hit;
            Ray ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(pos);
            Debug.DrawRay(ray.origin, ray.direction * 150, Color.red, 5);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
                selectedGo.transform.position = hit.point;
                selectedGo.transform.localPosition = new Vector3(RoundToNearestGrid(selectedGo.transform.localPosition.x), 0, RoundToNearestGrid(selectedGo.transform.localPosition.z));
            }
        }
    }

    float RoundToNearestGrid(float pos) => pos - pos % snapValue;
}

