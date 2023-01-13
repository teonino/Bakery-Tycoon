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
    [SerializeField] private Money money;
    [SerializeField] private Controller controller;
    [SerializeField] private AssetReference cursor;
    [SerializeField] private InterractQuest interractQuest;

    [Header("Global Parameters")]
    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private LayerMask putDownLayerFloor;
    [SerializeField] private LayerMask putDownLayerWall;
    [SerializeField] private float snapValue;

    [Header("Gamepad Parameters")]
    [SerializeField] private int cursorSpeed = 1;

    private FurnitureManager furnitureManager;
    private GameObject mainCamera;
    private GameObject buildingCamera;
    private GameObject previewCamera;
    private LayerMask currentRaycastlayer;
    private LayerMask initialGoLayer;
    private GameObject cursorObject;
    private GameObject selectedGo;
    private bool inBuildingMode = false;
    private float originalHeight;

    private void Start() {
        currentRaycastlayer = pickUpLayer;
        playerControllerSO.GetPlayerController().playerInput.Building.Sell.performed += Sell;
        playerControllerSO.GetPlayerController().playerInput.Building.Quit.performed += Quit;
        playerControllerSO.GetPlayerController().playerInput.Building.Select.performed += Select;
        playerControllerSO.GetPlayerController().playerInput.Building.Rotate.performed += RotateGameObject;
        playerControllerSO.GetPlayerController().playerInput.Building.EnablePreview.performed += EnablePreview;
        playerControllerSO.GetPlayerController().playerInput.Building.DisplayFurnitureStore.performed += DisplayFurtniturePanel;

        furnitureManager = FindObjectOfType<FurnitureManager>(true);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        buildingCamera = GameObject.FindGameObjectWithTag("BuildCamera");
        previewCamera = GameObject.FindGameObjectWithTag("PreviewCamera");

        buildingCamera.SetActive(false);
        previewCamera.SetActive(false);
    }

    public override void Effect() {
        if (day.GetDayTime() != DayTime.Day) {
            playerControllerSO.GetPlayerController().DisableInput();
            playerControllerSO.GetPlayerController().playerInput.Building.Enable();

            CreateCursor();
            //if (controller.IsGamepad())
            //    if (!cursorObject)
            //        cursor.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
            //            cursorObject = go.Result;
            //        };
            //    else
            //        cursorObject.SetActive(true);

            inBuildingMode = true;
            mainCamera.SetActive(false);
            buildingCamera.SetActive(true);
            furnitureManager.SetBuildingMode(this);

            interractQuest?.OnInterract();
        }
    }

    public void Sell(CallbackContext ctx) {
        if (ctx.performed && selectedGo) {
            if (selectedGo.TryGetComponent(out FurnitureHolder holder)) {
                money.AddMoney(holder.GetFurniturePrice());
                print($"Add {holder.GetFurniturePrice()} €");
            }
            else {
                print("Furniture not implemented");
            }
        }
    }

    public void Quit(CallbackContext context) {
        if (context.performed && !selectedGo) {
            cursorObject?.SetActive(false);
            playerControllerSO.GetPlayerController().playerInput.Building.Disable();
            playerControllerSO.GetPlayerController().EnableInput();
            mainCamera.SetActive(true);
            buildingCamera.SetActive(false);
            inBuildingMode = false;
        }
    }

    private void CreateCursor() {
        if (!cursorObject) {
            cursor.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
                cursorObject = go.Result;
                EnableCursor(true);
            };
        }
        else
            EnableCursor(true);
    }

    private void EnableCursor(bool active) {
        if (controller.IsGamepad())
            cursorObject.SetActive(active);
        else
            cursorObject.SetActive(false);
    }

    private void EnablePreview(CallbackContext ctx) {
        if (ctx.performed) {
            if (!previewCamera.activeSelf) {
                EnableCursor(false);
                buildingCamera.SetActive(false);
                previewCamera.SetActive(true);
            }
            else {
                EnableCursor(true);
                buildingCamera.SetActive(true);
                previewCamera.SetActive(false);
            }
        }
    }

    private void DisplayFurtniturePanel(CallbackContext context) {
        playerControllerSO.GetPlayerController().playerInput.Building.Disable();
        cursorObject?.SetActive(false);
        furnitureManager.gameObject.SetActive(true);
    }

    private void Select(CallbackContext context) {
        Ray ray;
        if (controller.IsGamepad())
            ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(cursorObject.transform.position);
        else
            ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
            if (!selectedGo) {
                selectedGo = hit.collider.gameObject;

                selectedGo.AddComponent<CheckCollisionManager>();
                selectedGo.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                selectedGo.GetComponent<CheckCollisionManager>().collidingMaterial = collidingMaterial;
                selectedGo.GetComponent<CheckCollisionManager>().layer = selectedGo.layer;

                if (selectedGo.layer == LayerMask.NameToLayer("CustomizableWall"))
                    currentRaycastlayer = putDownLayerWall;
                else
                    currentRaycastlayer = putDownLayerFloor;

                initialGoLayer = selectedGo.layer;
                selectedGo.layer = 3;
                originalHeight = selectedGo.transform.position.y;

                ChangeColliderSize(true);
            }
            else {
                if (selectedGo.GetComponent<CheckCollisionManager>().GetNbCollision() == 0) {

                    currentRaycastlayer = pickUpLayer;
                    selectedGo.layer = initialGoLayer;
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
        if (selectedGo && initialGoLayer != LayerMask.NameToLayer("CustomizableWall")) {
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
                selectedGo.transform.position = hit.point;

                if (initialGoLayer == LayerMask.NameToLayer("CustomizableWall")) {
                    selectedGo.transform.rotation = hit.transform.rotation;
                    selectedGo.transform.localPosition = new Vector3(selectedGo.transform.localPosition.x, originalHeight, selectedGo.transform.localPosition.z);
                }
                else
                    selectedGo.transform.localPosition = new Vector3(RoundToNearestGrid(selectedGo.transform.localPosition.x), originalHeight, RoundToNearestGrid(selectedGo.transform.localPosition.z));
            }
        }
    }

    float RoundToNearestGrid(float pos) => pos - pos % snapValue;

    private void OnDestroy() {
        if (cursorObject)
            Addressables.ReleaseInstance(cursorObject);

        playerControllerSO.GetPlayerController().playerInput.Building.Sell.performed -= Sell;
        playerControllerSO.GetPlayerController().playerInput.Building.Quit.performed -= Quit;
        playerControllerSO.GetPlayerController().playerInput.Building.Select.performed -= Select;
        playerControllerSO.GetPlayerController().playerInput.Building.Rotate.performed -= RotateGameObject;
        playerControllerSO.GetPlayerController().playerInput.Building.EnablePreview.performed -= EnablePreview;
        playerControllerSO.GetPlayerController().playerInput.Building.DisplayFurnitureStore.performed -= DisplayFurtniturePanel;
    }
}

