using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : Button {
    public DialogueManager dialogueManager;
    public Dialogue nextDialogue;
    public int relationReward;

    public void NextDialogue() {
        if (relationReward != 0)
            print("Relation : " + relationReward);

        if (!string.IsNullOrEmpty(nextDialogue.npcSpeech))
            dialogueManager.SetDialogue(nextDialogue);
        else
            dialogueManager.Destroy();
    }

    public void SetNextDialogue(Dialogue dialogue) => nextDialogue = dialogue;
    public void SetRelationReward(int value) => relationReward = value;
}
