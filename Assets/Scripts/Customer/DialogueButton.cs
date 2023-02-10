using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DialogueButton : Button {
    public DialogueManager dialogueManager;
    public int nextDialogue = 0;
    public int relationReward;

    public void NextDialogue() {
        if (relationReward != 0)
            print("Relation : " + relationReward);

        if (nextDialogue != 0)
            dialogueManager.SetAnswer(nextDialogue, relationReward);
        else
            dialogueManager.gameObject.SetActive(false);
    }

    public void SetNextDialogue(int id) => nextDialogue = id;
    public void SetRelationReward(int value) => relationReward = value;
}
