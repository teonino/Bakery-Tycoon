using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using System;

public class AmmountManager : MonoBehaviour {
    [SerializeField] private int ammountToBuy;
    [SerializeField] private TextMeshProUGUI textAmmount;
    [SerializeField] private RawImage imageProduct;
    [SerializeField] private PlayerControllerSO playerController;
    [SerializeField] private Controller controller;
    [SerializeField] private Animator upArrowAnimator;
    [SerializeField] private Animator downArrowAnimator;

    [HideInInspector] public DeliveryButton deliveryButton;
    [HideInInspector] public DeliveryManager deliveryManager;

    private ProductSO ProductSO;
    private IngredientSO Ingredient;
    private int originalAmmount = 0;

    private void Confirm(InputAction.CallbackContext ctx) {
        StartCoroutine(WaitForGamepad());
        deliveryButton.nbIngredient = ammountToBuy;
        originalAmmount = ammountToBuy;
    }

    private void Cancel(InputAction.CallbackContext ctx) {
        if (originalAmmount < ammountToBuy) {
            while(ammountToBuy != originalAmmount) {
                SetIngredientsInCart(false);
                ammountToBuy--;
            }
        } else {
            while (ammountToBuy != originalAmmount) {
                SetIngredientsInCart(true);
                ammountToBuy++;
            }
        }

        StartCoroutine(WaitForGamepad());
    }

    public void SetTexture(Texture texture) {
        imageProduct.texture = texture;
    }

    private void OnEnable() {
        deliveryManager = FindObjectOfType<DeliveryManager>();
        ammountToBuy = deliveryButton.nbIngredient;
        textAmmount.text = ammountToBuy.ToString();

        controller.RegisterCurrentSelectedButton();
        controller.SetEventSystemToStartButton(null);

        playerController.GetPlayerController().playerInput.UI.Disable();
        playerController.GetPlayerController().playerInput.Amafood.Disable();
        playerController.GetPlayerController().playerInput.Ammount.Enable();
        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.performed += PlusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.performed += MinusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.Confirm.performed += Confirm;
        //playerController.GetPlayerController().playerInput.Ammount.Cancel.performed += Cancel;
        playerController.GetPlayerController().playerInput.Ammount.Cancel.performed += Confirm;
    }

    private void OnDisable() {
        deliveryButton = null;

        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.performed -= PlusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.performed -= MinusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.Confirm.performed -= Confirm;
        //playerController.GetPlayerController().playerInput.Ammount.Cancel.performed -= Cancel;
        playerController.GetPlayerController().playerInput.Ammount.Cancel.performed -= Confirm;
        playerController.GetPlayerController().playerInput.Ammount.Disable();
        playerController.GetPlayerController().playerInput.Amafood.Enable();
        playerController.GetPlayerController().playerInput.UI.Enable();
    }

    private IEnumerator WaitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToLastButton();

        textAmmount.text = ammountToBuy.ToString();
        deliveryButton.tabs.canChangeTab = true;
        gameObject.SetActive(false);
    }

    public void MinusButtonIsClicked(InputAction.CallbackContext ctx) {
        if (ctx.performed && ammountToBuy > 0) {
            ammountToBuy -= 1;
            downArrowAnimator.SetTrigger("Move");
            SetIngredientsInCart(false);
        }
    }

    public void PlusButtonIsClicked(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            ammountToBuy += 1;
            downArrowAnimator.SetTrigger("Move");
            SetIngredientsInCart(true);
        }
    }

    private void SetIngredientsInCart(bool add) {
        if (deliveryButton.ingredient)
            deliveryManager.SetIngredient(deliveryButton.ingredient, add);
        else
            foreach (IngredientsForProduct ingredient in deliveryButton.product.ingredients) {
                deliveryManager.SetIngredient(ingredient.ingredient, add);
                deliveryButton.GetIngredientButton(ingredient.ingredient).nbIngredient++;
            }

        textAmmount.text = ammountToBuy.ToString();
    }

    public void SetTextAmount() {
        ammountToBuy++;
        textAmmount.text = ammountToBuy.ToString();
    }

    public int GetAmount() => ammountToBuy;
    public void ResetAmount() {
        ammountToBuy = 0;
        textAmmount.text = ammountToBuy.ToString();
    }
}
