using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoWorkstation : Workstation {
    [Header("Tutorial Parameters")]
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private InterractQuest interractQuest;
    [SerializeField] private InterractQuest secondInterractQuest;
    [SerializeField] private DialogueManager dialogueManager;

    private bool firstTime = true;
    public override void Effect() {
        if (tutorial.CanInterractWorkstation()) {
            interractQuest?.OnInterract();
            secondInterractQuest?.OnInterract();
            base.Effect();
        }
    }

    public override void CloseWorkplace(GameObject go) {
        base.CloseWorkplace(go);

        if(firstTime && dialogueManager.gameObject.activeSelf) {
            playerControllerSO.GetPlayerController().DisableInput();
            firstTime = false;
        }

        tutorial.Invoke();
    }
}
