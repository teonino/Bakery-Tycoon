using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI questTxt;
    private DialogueManager dialogueManager;
    private Dialogue dialogue;

    private void Awake() {
        dialogueManager = FindObjectOfType<DialogueManager>(true);

        dialogue = new Dialogue();
        dialogue.npcSpeech = "Welcome my dear little marmotte to your soon-to-become bakery ! Here you will prepare and serve delicious food to your customers. Today i will help you to prepare and find your way in the bakery !";
        dialogue.answers.Add(new Answer("Continue...", 0, new Dialogue()));
        dialogue.answers[0].nextDialogue.npcSpeech = "To bake your first baguette, get to the computer to order on amafood !";
        dialogue.answers[0].nextDialogue.answers.Add(new Answer("Continue...", 0, new Dialogue()));

        dialogueManager.SetDialogue(dialogue);
        dialogueManager.gameObject.SetActive(true);

        dialogueManager.OnDestroyDialoguePanel += NextTutorial;
    }

    private void NextTutorial() {
        questTxt.gameObject.SetActive(true);
        questTxt.text = "Go to your computer";
    }
}
