using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchListDelivery : MonoBehaviour {
    [SerializeField] private GameObject ingredientsList;
    [SerializeField] private GameObject productList;
    [SerializeField] private PlayerControllerSO playerControllerSO;

    private void Start() {

        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.performed += SwitchList;
    }

    private void OnEnable() {
        playerControllerSO.GetPlayerController().playerInput.Amafood.Enable();
    }

    private void OnDisable() {
        playerControllerSO.GetPlayerController().playerInput.Amafood.Disable();
    }

    private void OnDestroy() {

        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.performed -= SwitchList;
    }

    public void SwitchList(InputAction.CallbackContext context) {
        if (ingredientsList.activeSelf) {
            ingredientsList.SetActive(false);
            productList.SetActive(true);
        }
        else {
            ingredientsList.SetActive(true);
            productList.SetActive(false);
        }
    }
}
