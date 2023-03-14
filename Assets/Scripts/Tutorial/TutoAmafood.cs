using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoAmafood : DeliveryManager {
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private OrderQuest orderQuest;
    [SerializeField] private InterractQuest productAmafoodInterract;
    [SerializeField] private DialogueManager dialogueManager;

    protected override void Start() {
        base.Start();

        dialogueManager.OnDisableDialoguePanel += SetButtonForGamepadTutorial;

        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.Disable();
        playerControllerSO.GetPlayerController().playerInput.UI.Quit.Disable();
    }

    public void SetButtonForGamepadTutorial() {
        dialogueManager.OnDisableDialoguePanel -= SetButtonForGamepadTutorial;
        SetButtonForGamepad();
    }

    protected override IEnumerator waitForGamepad(GameObject obj) {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(obj);
        controller.RegisterCurrentSelectedButton();
        yield return new WaitForEndOfFrame();
        tutorial.Invoke();
    }

    public override void SetIngredient(IngredientSO ingredient, bool add) {
        base.SetIngredient(ingredient, add);
        if (orderQuest.CheckIngredient(ingredient)) {
            playerControllerSO.GetPlayerController().playerInput.Amafood.Disable();


            playerControllerSO.GetPlayerController().playerInput.Ammount.Confirm.performed -= FindObjectOfType<AmmountManager>().Confirm;
            playerControllerSO.GetPlayerController().playerInput.Ammount.Cancel.performed -= FindObjectOfType<AmmountManager>().Cancel;
            dialogueManager.OnDisableDialoguePanel += EnableAmafoodMap;
        }
    }

    public void EnableAmafoodMap() {
        playerControllerSO.GetPlayerController().playerInput.Amafood.Enable();

        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.Enable();
        playerControllerSO.GetPlayerController().playerInput.Ammount.Confirm.performed += FindObjectOfType<AmmountManager>().Confirm;
        playerControllerSO.GetPlayerController().playerInput.Ammount.Cancel.performed += FindObjectOfType<AmmountManager>().Cancel;
        dialogueManager.OnDisableDialoguePanel -= EnableAmafoodMap;
    }

    public override void DisplayProductList(InputAction.CallbackContext context) {
        base.DisplayProductList(context);
        productAmafoodInterract.OnInterract();
    }
}
