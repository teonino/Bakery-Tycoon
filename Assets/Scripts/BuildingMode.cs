using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class BuildingMode : Interactable {
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject buildingCamera;
    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private LayerMask putDownLayer;
    [SerializeField] private float snapValue;

    private LayerMask currentRaycastlayer;
    private LayerMask intitialGoLayer;

    private GameObject selectedGo;

    private void Start() {
        currentRaycastlayer = pickUpLayer;
        playerController.playerInput.Building.Quit.performed += Quit;
        playerController.playerInput.Building.Select.performed += Select;
    }

    public override void Effect() {
        playerController.DisableInput();
        playerController.playerInput.Building.Enable();
        mainCamera.SetActive(false);
        buildingCamera.SetActive(true);
    }

    public void Quit(CallbackContext context) {
        if (context.performed && !selectedGo) {
            playerController.playerInput.Building.Disable();
            playerController.EnableInput();
            mainCamera.SetActive(true);
            buildingCamera.SetActive(false);
        }
    }

    public void Select(CallbackContext context) {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentRaycastlayer))
            if (!selectedGo) {
                currentRaycastlayer = putDownLayer;
                selectedGo = hit.collider.gameObject;
                intitialGoLayer = selectedGo.layer;
                selectedGo.layer = 3;
                selectedGo.AddComponent<CheckCollision>();
                selectedGo.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                selectedGo.GetComponent<Collider>().isTrigger = true;
                print(selectedGo.name);
            }
            else {
                if (selectedGo.GetComponent<CheckCollision>().nbObjectInCollision == 0) {
                    currentRaycastlayer = pickUpLayer;
                    selectedGo.layer = intitialGoLayer;
                    Destroy(selectedGo.GetComponent<CheckCollision>());
                    Destroy(selectedGo.GetComponent<Rigidbody>());
                    selectedGo.GetComponent<Collider>().isTrigger = false;
                    selectedGo = null;
                }
                else
                    print("Colliding");
            }
    }

    private void FixedUpdate() {
        if (selectedGo) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, Mathf.Infinity, currentRaycastlayer))
                selectedGo.transform.position = new Vector3(
                    RoundToNearestGrid(hit.point.x),
                    selectedGo.transform.localScale.y / 2,
                    RoundToNearestGrid(hit.point.z));
        }
    }

    float RoundToNearestGrid(float pos) => pos - pos % snapValue;
}

