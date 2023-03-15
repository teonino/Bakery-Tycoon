using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRecipeBook : RecipeBookManager {

    [SerializeField] private Tutorial tutorial;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private WorkstationManager workstationManager;

    private bool firstTime = true;

    protected override void OnEnable() {
        base.OnEnable();
        if (tutorial && firstTime && dialogueManager.gameObject.activeSelf && workstationManager.gameObject.activeSelf) {
            playerController.GetPlayerController().playerInput.RecipeBook.Disable();
            playerController.GetPlayerController().playerInput.Workstation.Disable();
            dialogueManager.OnDisableDialoguePanel += EnableInputs;
            firstTime = false;
        }
    }

    private void EnableInputs() {
        playerController.GetPlayerController().playerInput.RecipeBook.Enable();
        playerController.GetPlayerController().playerInput.Workstation.Enable();
        dialogueManager.OnDisableDialoguePanel -= EnableInputs;
    }
}
