using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : Button {
    public DialogueManager dialogueManager;
    public int nextDialogueID;
    public int relationReward;

    public void NextDialogue() {
        print("Id : " + nextDialogueID + " / Relation : " + relationReward);

        if (nextDialogueID != 0)
            dialogueManager.SetDialogue(nextDialogueID);
        else 
            dialogueManager.Destroy();
    }

    public void SetNextDialogueID(int value) => nextDialogueID = value;
    public void SetRelationReward(int value) => relationReward = value;
}
