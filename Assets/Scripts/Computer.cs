using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Computer : Interactable {
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject buildingCamera;
    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private LayerMask putDownLayer;

    private LayerMask layerCurrent;

    private GameObject selectedGo;

    private void Start() {
        layerCurrent = pickUpLayer;
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
        if (context.performed) {
            playerController.playerInput.Building.Disable();
            playerController.EnableInput();
            mainCamera.SetActive(true);
            buildingCamera.SetActive(false);
        }
    }



    public void Select(CallbackContext context) {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerCurrent))
            if (!selectedGo) {
                layerCurrent = putDownLayer;
                selectedGo = hit.collider.gameObject;
                selectedGo.layer = 3;
                print(selectedGo.name);
            }
            else  {
                layerCurrent = pickUpLayer;
                selectedGo.layer = 0;
                selectedGo = null;
                print("empty");
            }
    }

    private void FixedUpdate() {
        if (selectedGo) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, Mathf.Infinity, layerCurrent))
                selectedGo.transform.position = hit.point + new Vector3(0, selectedGo.transform.localScale.y / 2, 0);
        }
    }
}

