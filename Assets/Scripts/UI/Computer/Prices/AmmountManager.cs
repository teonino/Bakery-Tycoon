using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using System;

public class AmmountManager : MonoBehaviour {
    [SerializeField] internal int ammountToBuy;
    [SerializeField] private int maxAmmountToBuy;
    [SerializeField] private TextMeshProUGUI textAmmount;
    [SerializeField] private RawImage imageProduct;
    [SerializeField] private PlayerControllerSO playerController;
    [SerializeField] private Controller controller;
    [SerializeField] private Animator downArrowAnimator;
    [SerializeField] private Animator upArrowAnimator;
    [SerializeField] private List<Vector2> inCartList = new List<Vector2>();


    [SerializeField] private AudioSource popSource;
    [SerializeField] private AudioClip popClip;
    [SerializeField] private AudioMixer mixer;

    private bool canUp;
    private bool canDown;
    private int timesToTest;

    [HideInInspector] public DeliveryButton deliveryButton;
    [HideInInspector] public DeliveryManager deliveryManager;

    private ProductSO ProductSO;
    private IngredientSO Ingredient;
    private DialogueManager dialogueManager;
    private TutoAmafood tutoAmafood;
    public int originalAmmount = 0;

    private void Start() {
        dialogueManager = FindObjectOfType<DialogueManager>(true);
        tutoAmafood = FindObjectOfType<TutoAmafood>();

        if (tutoAmafood)
            dialogueManager.OnDisableDialoguePanel += tutoAmafood.SetButtonForGamepadTutorial;
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
        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.canceled += ReleaseButton;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.performed += MinusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.canceled += ReleaseButton;
        playerController.GetPlayerController().playerInput.Ammount.Confirm.performed += Confirm;
        playerController.GetPlayerController().playerInput.Ammount.Cancel.performed += Cancel;
        //playerController.GetPlayerController().playerInput.Ammount.Cancel.performed += Confirm;
    }

    public void Confirm(InputAction.CallbackContext ctx) {
        StartCoroutine(WaitForGamepad());
        deliveryButton.nbIngredient = ammountToBuy;
        originalAmmount = ammountToBuy;
    }

    public void Cancel(InputAction.CallbackContext ctx) {
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

    private void OnDisable() {
        deliveryButton = null;

        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.performed -= PlusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.AddIngredient.canceled -= ReleaseButton;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.performed -= MinusButtonIsClicked;
        playerController.GetPlayerController().playerInput.Ammount.RemoveIngredient.canceled -= ReleaseButton;
        playerController.GetPlayerController().playerInput.Ammount.Confirm.performed -= Confirm;
        playerController.GetPlayerController().playerInput.Ammount.Cancel.performed -= Cancel;
        //playerController.GetPlayerController().playerInput.Ammount.Cancel.performed -= Confirm;
        playerController.GetPlayerController().playerInput.Ammount.Disable();
        playerController.GetPlayerController().playerInput.Amafood.Enable();
        playerController.GetPlayerController().playerInput.UI.Enable();
    }

    private void ReleaseButton(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            canDown = false;
            canUp = false;
            timesToTest = 0;
            StopAllCoroutines();
            upArrowAnimator.SetTrigger("MoveToIdle");
            upArrowAnimator.SetTrigger("MoveToIdle");
        }
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
            //ammountToBuy -= 1;
            canDown = true;
            canUp = false;
            StartCoroutine(DecrementAmmountToBuy());
        }
    }

    public void PlusButtonIsClicked(InputAction.CallbackContext ctx) {
        if (ctx.performed && ammountToBuy < maxAmmountToBuy) {
            //ammountToBuy += 1;
            canUp = true;
            canDown = false;
            StartCoroutine(IncrementAmmountToBuy());
        }
    }

    private IEnumerator IncrementAmmountToBuy()
    {
        if (canUp && ammountToBuy < maxAmmountToBuy)
        {
            if (timesToTest < 3)
            {
                upArrowAnimator.SetTrigger("MoveUp");
                popSource.PlayOneShot(popClip);
                timesToTest += 1;
                ammountToBuy += 1;
                textAmmount.text = ammountToBuy.ToString();
                SetIngredientsInCart(true);
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(IncrementAmmountToBuy());
            }
            else if (timesToTest >= 3)
            {
                StartCoroutine(IncrementFastAmmountToBuy());
                timesToTest = 0;
            }
            yield return null;
        } 
    }

    private IEnumerator IncrementFastAmmountToBuy()
    {
        if (canUp && ammountToBuy < maxAmmountToBuy)
        {
            yield return new WaitForSeconds(0.1f);
            upArrowAnimator.SetTrigger("MoveUp");
            popSource.PlayOneShot(popClip);
            ammountToBuy += 1;
            textAmmount.text = ammountToBuy.ToString(); 
            SetIngredientsInCart(true);
            StartCoroutine(IncrementFastAmmountToBuy());
        }
        yield return null;
    }

    private IEnumerator DecrementAmmountToBuy()
    {
        if (canDown && ammountToBuy > 0)
        {
            if (timesToTest < 3)
            {
                downArrowAnimator.SetTrigger("MoveDown");
                popSource.PlayOneShot(popClip);
                timesToTest += 1;
                ammountToBuy -= 1;
                textAmmount.text = ammountToBuy.ToString(); 
                SetIngredientsInCart(false);
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(DecrementAmmountToBuy());
            }
            else if (timesToTest >= 3)
            {
                StartCoroutine(DecrementFastAmmountToBuy());
                timesToTest = 0;
            }
            yield return null;
        }
    }

    private IEnumerator DecrementFastAmmountToBuy()
    {
        if (canDown && ammountToBuy > 0)
        {
            yield return new WaitForSeconds(0.1f);
            downArrowAnimator.SetTrigger("MoveDown");
            popSource.PlayOneShot(popClip);
            ammountToBuy -= 1;
            textAmmount.text = ammountToBuy.ToString();
            SetIngredientsInCart(false);
            StartCoroutine(DecrementFastAmmountToBuy());
        }
        yield return null;
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
