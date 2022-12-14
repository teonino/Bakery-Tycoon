using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI questTxt;
    [SerializeField] private List<Quest> quests;

    private int indexQuest = 0;
    private DialogueManager dialogueManager;
    private Dialogue dialogue;

    private void Awake() {
        foreach (Quest quest in quests)
            quest.SetActive(false);

        dialogueManager = FindObjectOfType<DialogueManager>(true);

        SetupDialogue();
        SetupQuest();
    }

    private void SetupDialogue() {
        dialogueManager.gameObject.SetActive(true);
        dialogueManager.GetDialogues(indexQuest + 1, "Tutorial");

        dialogueManager.OnDestroyDialoguePanel += LaunchQuest;

        //dialogue = new Dialogue();
        //dialogue.npcSpeech = "Welcome my dear little marmotte to your soon-to-become bakery ! Here you will prepare and serve delicious food to your customers. Today i will help you to prepare and find your way in the bakery !";
        //dialogue.answers.Add(new Answer("Continue...", 0, new Dialogue()));
        //dialogue.answers[0].nextDialogue.npcSpeech = "To bake your first baguette, get to the computer to order on amafood !";
        //dialogue.answers[0].nextDialogue.answers.Add(new Answer("Continue...", 0, new Dialogue()));

        //dialogueManager.SetDialogue(dialogue);

    }

    private void SetupQuest() {
        quests[indexQuest].OnCompletedAction += NextQuest;
        quests[indexQuest].SetActive(true);
        LaunchQuest();
    }

    private void LaunchQuest() {
        if (indexQuest < quests.Count) {
            questTxt.gameObject.SetActive(true);
            quests[indexQuest].UpdateUI(questTxt);
        }
        else
            SceneManager.LoadScene("MainMenu");
    }

    private void NextQuest() {
        indexQuest++;
        if (indexQuest < quests.Count) {
            SetupDialogue();
            SetupQuest();
        }
        else {
            questTxt.gameObject.SetActive(false);
            SetupDialogue();
        }
    }
}
