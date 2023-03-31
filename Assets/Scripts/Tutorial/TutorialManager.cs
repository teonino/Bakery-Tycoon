using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTxt;
    [SerializeField] private List<Quest> quests;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private Day day;

    private int indexQuest = 0;
    private DialogueManager dialogueManager;

    private void Awake()
    {
        foreach (Quest quest in quests)
            quest.SetActive(false);

        dialogueManager = FindObjectOfType<DialogueManager>(true);
        dialogueManager.OnDestroyDialoguePanel += LaunchQuest;
        tutorial.action += SetDefaultButton;

        SetupDialogue();
        SetupQuest();
    }

    private void OnDestroy()
    {
        tutorial.action -= SetDefaultButton;
        dialogueManager.OnDestroyDialoguePanel -= LaunchQuest;
    }

    private void SetupDialogue()
    {
        dialogueManager.gameObject.SetActive(true);
        dialogueManager.GetDialogues(indexQuest + 1, "Tutorial");
    }

    private void LaunchQuest()
    {
        if (indexQuest < quests.Count)
        {
            questTxt.gameObject.SetActive(true);
            quests[indexQuest].UpdateUI(questTxt);
        }
        else
            SceneManager.LoadScene("MainMenu_reworked");
    }

    private void NextQuest()
    {
        quests[indexQuest].OnCompletedAction -= NextQuest;
        indexQuest++;
        if (indexQuest < quests.Count)
        {
            SetupDialogue();
            SetupQuest();
            if (indexQuest == quests.Count - 1)
                day?.OnNextDayPhase();
        }
        else
        {
            questTxt.gameObject.SetActive(false);
            SetupDialogue();
        }
    }
    private void SetupQuest()
    {
        quests[indexQuest].OnCompletedAction += NextQuest;
        quests[indexQuest].SetActive(true);
        LaunchQuest();
    }

    private void SetDefaultButton() {
        dialogueManager.SetDefaultButton();
    }
}
