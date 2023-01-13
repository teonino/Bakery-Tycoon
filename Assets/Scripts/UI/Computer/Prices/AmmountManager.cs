using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class AmmountManager : MonoBehaviour {
    [SerializeField] private int amountToBuy;
    [SerializeField] private TextMeshProUGUI textAmmount;
    [SerializeField] public DeliveryButton deliveryButton;
    [SerializeField] public PlayerControllerSO playerController;
    [SerializeField] public Controller controller;

    public DeliveryManager deliveryManager;

    private void Start() {
        deliveryManager = FindObjectOfType<DeliveryManager>();
        amountToBuy = deliveryButton.nbIngredient;      
    }

    private void Confirm(InputAction.CallbackContext ctx) {
        StartCoroutine(WaitForGamepad());
        gameObject.SetActive(false);
    }

    private void Cancel(InputAction.CallbackContext ctx) {
        for (int i = amountToBuy; i > 0; i--, amountToBuy--) 
            SetIngredientsInCart(false);

        StartCoroutine(WaitForGamepad());
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        controller.RegisterCurrentSelectedButton();
        controller.SetEventSystemToStartButton(null);

        playerController.GetPlayerController().playerInput.UI.Disable();
        playerController.GetPlayerController().playerInput.Ammount.Enable();
        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.performed += PlusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.performed += MinusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.Confirm.performed += Confirm;
        playerController.GetPlayerController().playerInput.Ammount.Cancel.performed += Cancel;
    }

    private void OnDisable() {
        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.performed -= PlusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.performed -= MinusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.Confirm.performed -= Confirm;
        playerController.GetPlayerController().playerInput.Ammount.Cancel.performed -= Cancel;
        playerController.GetPlayerController().playerInput.Ammount.Disable();
        playerController.GetPlayerController().playerInput.UI.Enable();
    }

    private IEnumerator WaitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToLastButton();
    }

    public void MinusButtonIsClicked(InputAction.CallbackContext ctx) {
        if (ctx.performed && amountToBuy > 0) {
            amountToBuy -= 1;
            SetIngredientsInCart(false);
        }
    }

    public void PlusButtonIsClicked(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            amountToBuy += 1;
            SetIngredientsInCart(true);
        }
    }

    private void SetIngredientsInCart(bool add) {
        if (deliveryButton.ingredient)
            deliveryManager.SetIngredient(deliveryButton.ingredient, add);
        else
            foreach (IngredientSO ingredient in deliveryButton.product.ingredients) {
                deliveryManager.SetIngredient(ingredient, add);
                deliveryButton.GetIngredientButton(ingredient).GetComponentInChildren<AmmountManager>().SetTextAmount();
            }

        textAmmount.text = amountToBuy.ToString();
    }

    public void SetTextAmount() {
        amountToBuy++;
        textAmmount.text = amountToBuy.ToString();
    }

    public int GetAmount() => amountToBuy;
    public void ResetAmount() {
        amountToBuy = 0;
        textAmmount.text = amountToBuy.ToString();
    }
}
