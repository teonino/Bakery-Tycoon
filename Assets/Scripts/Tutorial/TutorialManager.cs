using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI questTxt;
    [SerializeField] private List<Quest> quests;
    [SerializeField] private Tutorial tutorial;

    private int indexQuest = 0;
    private DialogueManager dialogueManager;
    private Dialogue dialogue;

    private void Awake() {
        tutorial.SetTutorial(true);

        foreach (Quest quest in quests)
            quest.SetActive(false);

        dialogueManager = FindObjectOfType<DialogueManager>(true);
        dialogueManager.OnDestroyDialoguePanel += LaunchQuest;

        SetupDialogue();
        SetupQuest();
    }

    private void SetupDialogue() {
        dialogueManager.gameObject.SetActive(true);
        dialogueManager.GetDialogues(indexQuest + 1, "Tutorial");
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
    private void SetupQuest() {
        quests[indexQuest].OnCompletedAction += NextQuest;
        quests[indexQuest].SetActive(true);
        LaunchQuest();
    }
}
