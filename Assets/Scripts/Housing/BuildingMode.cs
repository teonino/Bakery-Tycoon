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
    [SerializeField] private LayerMask putDownLayerFrame;
    [SerializeField] private float snapValue;
    [SerializeField] private ParticleSystem vfx;

    [Header("Gamepad Parameters")]
    [SerializeField] private int cursorSpeed = 1;

    [SerializeField] private SFXPlayer sfxPlayer;
    private FurnitureManager furnitureManager;
    private GameObject level;
    private GameObject mainCamera;
    private GameObject buildingCamera;
    private LayerMask currentRaycastlayer;
    private LayerMask initialGoLayer;
    private GameObject cursorObject;
    private GameObject selectedGo;
    private GameObject disabledGo;
    private bool inBuildingMode = false;
    private bool selectedGoIsFloor = false;
    private bool selectedGoIsWall = false;
    private bool selectedGoIsFrame = false;
    private bool selectedGoIsBought = false;
    private float originalHeight;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    [SerializeField] private List<GameObject> uiInGame;
    [SerializeField] private GameObject uiCustomisation;
    [SerializeField] private GameObject popUpCustomUnavaible;

    protected override void Start() {
        currentRaycastlayer = pickUpLayer;
        playerControllerSO.GetPlayerController().playerInput.Building.Sell.performed += Sell;
        playerControllerSO.GetPlayerController().playerInput.Building.Quit.performed += Quit;
        playerControllerSO.GetPlayerController().playerInput.Building.Select.performed += Select;
        playerControllerSO.GetPlayerController().playerInput.Building.Rotate.performed += RotateGameObject;
        playerControllerSO.GetPlayerController().playerInput.Building.DisplayFurnitureStore.performed += DisplayFurtniturePanel;

        furnitureManager = FindObjectOfType<FurnitureManager>(true);
        level = GameObject.FindGameObjectWithTag("Level");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        buildingCamera = GameObject.FindGameObjectWithTag("BuildCamera");
        vfx.Stop();

        buildingCamera.SetActive(false);
    }

    public override void Effect() {
        if (day.GetDayTime() == DayTime.Evening) {
            playerControllerSO.GetPlayerController().DisableInput();
            playerControllerSO.GetPlayerController().playerInput.Building.Enable();
            sfxPlayer.InteractSound();
            CreateCursor();
            vfx.Play();



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

            for (int i = 0; i < uiInGame.Count; i++) {
                uiInGame[i].SetActive(false);
            }
            uiCustomisation.SetActive(true);
        }
        else
        {
            StartCoroutine(DisplayPopUp());
        }
    }
    public override bool CanInterract() {
        canInterract = day.GetDayTime() != DayTime.Day;
        return canInterract;
    }

    private IEnumerator DisplayPopUp()
    {
        popUpCustomUnavaible?.SetActive(true);
        //playerControllerSO.GetPlayerController().playerInput.Disable();
        yield return new WaitForSeconds(2);
        //playerControllerSO.GetPlayerController().playerInput.Enable();
        popUpCustomUnavaible?.SetActive(false);
    }

    public void Sell(CallbackContext ctx) {
        Sell();
    }

    private void Sell() {
        if (selectedGo) {
            if (selectedGo.TryGetComponent(out FurnitureHolder holder)) {
                if (holder.CanRemoveSelectedItem()) {
                    if (!selectedGoIsBought) {
                        money.AddMoney(holder.GetFurniturePrice());
                        print($"Sold {holder.name} => Add {holder.GetFurniturePrice()} €");
                    }
                    Destroy(holder.gameObject);
                    currentRaycastlayer = pickUpLayer;
                    selectedGo = null;

                    if (disabledGo) {
                        disabledGo.SetActive(true);
                        disabledGo = null;
                    }
                }
                else
                    print("Furniture mandatory, you can't sell this unless you have another one");
            }
            else {
                print("Furniture not implemented");
            }
        }
    }

    public void Quit(CallbackContext context) {
        if (!selectedGo) {
            cursorObject?.SetActive(false);
            playerControllerSO.GetPlayerController().playerInput.Building.Disable();
            playerControllerSO.GetPlayerController().EnableInput();
            mainCamera.SetActive(true);
            buildingCamera.SetActive(false);
            inBuildingMode = false;
            for (int i = 0; i < uiInGame.Count; i++) {
                uiInGame[i].SetActive(true);
            }
            uiCustomisation.SetActive(false);
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

    private void DisplayFurtniturePanel(CallbackContext context) {
        playerControllerSO.GetPlayerController().playerInput.Building.Disable();
        cursorObject?.SetActive(false);
        furnitureManager.gameObject.SetActive(true);
    }

    private void Select(CallbackContext context) {
        if (selectedGoIsWall || selectedGoIsFloor && selectedGo) {
            Destroy(disabledGo);
            money.AddMoney(-selectedGo.GetComponent<FurnitureHolder>().GetFurniturePrice());
            currentRaycastlayer = pickUpLayer;
            selectedGo.layer = initialGoLayer;
            selectedGoIsFloor = selectedGoIsWall = false;
            selectedGo = null;
        }
        else {
            Ray ray;
            if (controller.IsGamepad())
                ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(cursorObject.transform.position);
            else
                ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
                if (!selectedGo) {
                    selectedGoIsBought = false;
                    originalPosition = hit.collider.transform.position;
                    originalRotation = hit.collider.transform.rotation;
                    SetSelectedGO(hit.collider.gameObject);
                }
                else if (selectedGo.GetComponent<CheckCollisionManager>().GetNbCollision() == 0) {
                    if (selectedGoIsBought)
                        money.AddMoney(-selectedGo.GetComponent<FurnitureHolder>().GetFurniturePrice());
                    ResetValue();
                }
            }
        }
    }

    private void ResetValue() {
        Destroy(selectedGo.GetComponent<CheckCollisionManager>());
        Destroy(selectedGo.GetComponent<Rigidbody>());
        currentRaycastlayer = pickUpLayer;
        selectedGo.layer = initialGoLayer;
        selectedGo.transform.parent = level.transform;
        selectedGoIsFloor = selectedGoIsWall = selectedGoIsFrame = false;
        selectedGoIsBought = false;
        ChangeColliderSize(false);
        selectedGo = null;
    }

    public void SetSelectedGO(GameObject go, bool bought = false) {
        if (selectedGo) {
            if (disabledGo)
                disabledGo.SetActive(true);

            if (selectedGoIsBought) {
                Sell();
                Addressables.ReleaseInstance(selectedGo);
            }
            else {
                selectedGo.transform.position = originalPosition;
                selectedGo.transform.rotation = originalRotation;
                ResetValue();
            }
        }

        selectedGoIsBought = bought;
        selectedGo = go;

        FurnitureType type = selectedGo.GetComponent<FurnitureHolder>().GetFurniture().GetType();

        if (type == FurnitureType.Wall) {
            selectedGoIsWall = true;
        }
        else if (type == FurnitureType.Floor) {
            selectedGoIsFloor = true;
        }
        else if (type == FurnitureType.Frame)
            selectedGoIsFrame = true;
        else {
            selectedGo.AddComponent<CheckCollisionManager>();
            selectedGo.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            selectedGo.GetComponent<CheckCollisionManager>().collidingMaterial = collidingMaterial;
            selectedGo.GetComponent<CheckCollisionManager>().layer = selectedGo.layer;

            ChangeColliderSize(true);
        }

        if (selectedGo.layer == LayerMask.NameToLayer("CustomizableWall") || (selectedGo.layer == LayerMask.NameToLayer("Walls") && type == FurnitureType.Wall))
            currentRaycastlayer = putDownLayerWall;
        else if (selectedGo.layer == LayerMask.NameToLayer("Frame"))
            currentRaycastlayer = putDownLayerFrame;
        else
            currentRaycastlayer = putDownLayerFloor;

        initialGoLayer = selectedGo.layer;
        selectedGo.layer = 3;
        originalHeight = selectedGo.transform.position.y;
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

            Vector2 movement = playerControllerSO.GetPlayerController().playerInput.Building.Move.ReadValue<Vector2>() * cursorSpeed;
            if (selectedGoIsWall || selectedGoIsFrame || selectedGoIsFloor) {
                if (controller.IsGamepad() && cursorObject) {
                    if (cursorObject.transform.position.x - movement.x > 0 && cursorObject.transform.position.y - movement.y > 0 && cursorObject.transform.position.x + movement.x < Screen.currentResolution.width && cursorObject.transform.position.y + movement.y < Screen.currentResolution.height) {
                        cursorObject.transform.Translate(playerControllerSO.GetPlayerController().playerInput.Building.Move.ReadValue<Vector2>() * cursorSpeed);
                        Replace(cursorObject.transform.position);
                    }
                }
                else {
                    Replace(Mouse.current.position.ReadValue());
                }
            }
            else {
                if (controller.IsGamepad() && cursorObject) {
                    if (cursorObject.transform.position.x  + movement.x > 0 && cursorObject.transform.position.y + movement.y > 0 && cursorObject.transform.position.x + movement.x < Screen.currentResolution.width && cursorObject.transform.position.y + movement.y < Screen.currentResolution.height) {
                        cursorObject.transform.Translate(playerControllerSO.GetPlayerController().playerInput.Building.Move.ReadValue<Vector2>() * cursorSpeed);
                        SnapGameObject(cursorObject.transform.position);
                    }
                }
                else {
                    SnapGameObject(Mouse.current.position.ReadValue());
                }
            }
        }
    }

    private void RotateGameObject(InputAction.CallbackContext ctx) {
        if (selectedGo && !selectedGoIsWall && initialGoLayer != LayerMask.NameToLayer("CustomizableWall")) {
            if (playerControllerSO.GetPlayerController().playerInput.Building.Rotate.ReadValue<float>() > 0) {
                selectedGo.transform.Rotate(Vector3.up * 90);
            }
            else if (playerControllerSO.GetPlayerController().playerInput.Building.Rotate.ReadValue<float>() < 0) {
                selectedGo.transform.Rotate(Vector3.up * -90);
            }
        }
    }

    private void Replace(Vector3 pos) {
        if (selectedGo) {
            RaycastHit hit;
            Ray ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
                if (disabledGo)
                    disabledGo.SetActive(true);
                disabledGo = hit.collider.gameObject;
                disabledGo.SetActive(false);
                selectedGo.transform.position = hit.collider.transform.position;

                if (selectedGoIsWall || selectedGoIsFrame)
                    selectedGo.transform.rotation = hit.collider.transform.rotation;
            }
        }
    }

    private void SnapGameObject(Vector3 pos) {
        if (selectedGo) {
            RaycastHit hit;
            Ray ray = buildingCamera.GetComponent<Camera>().ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer)) {
                selectedGo.transform.position = hit.point; //Set position to hitpoint

                if (initialGoLayer == LayerMask.NameToLayer("CustomizableWall")) {
                    selectedGo.transform.rotation = hit.transform.rotation;
                    selectedGo.transform.localPosition = new Vector3(selectedGo.transform.localPosition.x, originalHeight, selectedGo.transform.localPosition.z); //set correct height
                }
                else {
                    selectedGo.transform.localPosition = new Vector3(RoundToNearestGrid(selectedGo.transform.localPosition.x), originalHeight, RoundToNearestGrid(selectedGo.transform.localPosition.z));
                }
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
        playerControllerSO.GetPlayerController().playerInput.Building.DisplayFurnitureStore.performed -= DisplayFurtniturePanel;
    }
}

