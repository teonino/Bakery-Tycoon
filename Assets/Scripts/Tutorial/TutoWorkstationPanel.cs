using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoWorkstationPanel : WorkstationManager {
    [SerializeField] private InterractQuest addIngredientQuest;
    [SerializeField] private CreateQuest cookQuest;
    [SerializeField] private InterractQuest createPasteQuest;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private DialogueManager dialogueManager;
    private bool canCook = true;
    private int timeOpenning = 0;
    private bool firstTimeAdding = true;
    private bool firstTimeCreate = true;

    protected override void OnEnable() {
        base.OnEnable();

        if (timeOpenning < 2 && dialogueManager.gameObject.activeSelf) {
            playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
            playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
            dialogueManager.OnDisableDialoguePanel += EnableQuit;
            timeOpenning++;
        }
    }

    private void EnableQuit() {
        dialogueManager.OnDisableDialoguePanel -= EnableQuit;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Enable();
        playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed += Quit;
    }

    protected override void SetupButton() {
        base.SetupButton();
        tutorial?.Invoke();
    }

    protected override void Update() {
        base.Update();
        if (gameObject.activeSelf && playerControllerSO.GetPlayerInputState())
            playerControllerSO?.GetPlayerController().DisableInput();
    }

    public override void IngredientSelected(IngredientSO ingredient) {
        if (addIngredientQuest.OnInterract()) {
            tutorial.UnlockAddIngredient();

            if (firstTimeAdding) {
                playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
                playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
                dialogueManager.OnDisableDialoguePanel += EnableQuit;
                firstTimeAdding = false;
            }
        }

        if (tutorial.CanAddIngredient())
            base.IngredientSelected(ingredient);
    }

    public override void Cook(InputAction.CallbackContext ctx) {
        if (CheckProduct(cookQuest.GetProduct())) {
            cookQuest.Completed();
            canCook = true;
        }

        if (canCook)
            base.Cook(ctx);
    }

    protected override void CreateProduct() {
        createPasteQuest?.OnInterract();

        base.CreateProduct();

        playerControllerSO.GetPlayerController().DisableInput();
        dialogueManager.OnDisableDialoguePanel += EnablePlayer;
    }

    private void EnablePlayer() {
        dialogueManager.OnDisableDialoguePanel -= EnablePlayer;
        playerControllerSO.GetPlayerController().EnableInput();
    }
}
