using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    [SerializeField] private LocalizedStringComponent npcSpeechTxt;
    [SerializeField] private TextMeshProUGUI npcNameTxt;
    [SerializeField] private List<LocalizedStringComponent> playerAnswersTxt;
    [SerializeField] private List<DialogueButton> playerAnswersButtons;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private CharacterTables characterTables;
    [SerializeField] private LocalizedStringTable tutorialDialogueTable;
    [SerializeField] private Tutorial tutorial;

    private string npcName;
    private int idDialogue;
    private int tutorialId = 1;
    private RegularSO currentRegular;

    public Action OnDestroyDialoguePanel;

    private void OnEnable() {
        if (playerControllerSO.GetPlayerController()) {
            playerControllerSO.GetPlayerController().DisableInput();
            playerControllerSO.GetPlayerController().playerInput.Dialogue.Enable();
            playerControllerSO.GetPlayerController().playerInput.Dialogue.Dialogue1.performed += InvokeButton1;
            playerControllerSO.GetPlayerController().playerInput.Dialogue.Dialogue2.performed += InvokeButton2;
            playerControllerSO.GetPlayerController().playerInput.Dialogue.Dialogue3.performed += InvokeButton3;
            controller.RegisterCurrentSelectedButton();
            Time.timeScale = 0;
        }
    }
    private void Start() {
        playerControllerSO.GetPlayerController().DisableInput();
        controller.RegisterCurrentSelectedButton();
        Time.timeScale = 0;


        for (int i = 0; i < playerAnswersTxt.Count; i++)
            playerAnswersTxt[0].GetComponent<DialogueButton>().tutorial = tutorial;
    }

    public void GetDialogues(int id, string character, RegularSO regular = null) {
        currentRegular = regular;
        npcName = character;
        idDialogue = id;
        if (!tutorial.GetTutorial())
            SetDialogue(id);
        else
            SetTutorialDialogue();
    }

    /*Old dialogue code
     * dialogue = new Dialogue();

try {
    StreamReader s = new StreamReader($"Assets\\Dialogues\\{character}\\{character}{id}.csv");

    int answerDialogueId = 0;
    bool npcSpeechNext = true;
    Dialogue currentDialogue = dialogue;
    while (!s.EndOfStream) {
        string line = s.ReadLine();

        if (line == "") {
            currentDialogue = dialogue.answers[answerDialogueId].nextDialogue;
            answerDialogueId++;
            npcSpeechNext = true;
        }
        else if (npcSpeechNext) {
            currentDialogue.npcSpeech = line;
            npcSpeechNext = false;
        }
        else {
            string[] values = line.Split("/");
            if (values.Length == 2)
                currentDialogue.answers.Add(new Answer(values[0], int.Parse(values[1]), new Dialogue()));
            else
                currentDialogue.answers.Add(new Answer(values[0], 0, new Dialogue()));
        }
    }
    s.Close();
}
catch (Exception e) {
    print("Error, Dialoge Manager... " + e.Message);
}
*/

    public void SetTutorialDialogue() {
        LocalizedStringTable table = tutorialDialogueTable;

        npcNameTxt.SetText(npcName);

        npcSpeechTxt.enabled = false;
        npcSpeechTxt.SetTable(table);
        npcSpeechTxt.SetKey("Speech" + tutorialId + "." + 1);
        npcSpeechTxt.enabled = true;

        string key = "Answer";
        playerAnswersTxt[0].transform.parent.gameObject.SetActive(true);
        playerAnswersTxt[0].enabled = false;
        playerAnswersTxt[0].SetTable(table);
        playerAnswersTxt[0].SetKey(key);
        playerAnswersTxt[0].enabled = true;


        DialogueButton button = playerAnswersTxt[0].GetComponent<DialogueButton>();
        button.dialogueManager = this;
        button.SetTutorial(tutorial);

        if (table.GetTable().GetEntry("Speech" + tutorialId + "." + 2) != null) {
            button.SetNextDialogue(1);
        }
        else {
            button.SetNextDialogue(0);
        }

        tutorialId++;
        StartCoroutine(WaitForGamepad(button.gameObject));
    }

    public void SetDialogue(int id) {
        LocalizedStringTable table = characterTables.GetTable(npcName);

        if (table == null)
            table = tutorialDialogueTable;

        npcNameTxt.SetText(npcName);

        npcSpeechTxt.enabled = false;
        npcSpeechTxt.SetTable(table);
        npcSpeechTxt.SetKey("Speech" + id + "." + 1);
        npcSpeechTxt.enabled = true;

        for (int i = 0; i < playerAnswersTxt.Count; i++) {
            string key = "Answer" + id + "." + (i + 1);

            playerAnswersTxt[i].gameObject.SetActive(true);
            playerAnswersTxt[i].enabled = false;
            playerAnswersTxt[i].SetTable(table);
            playerAnswersTxt[i].SetKey(key);
            playerAnswersTxt[i].enabled = true;

            DialogueButton button = playerAnswersTxt[i].GetComponent<DialogueButton>();
            button.dialogueManager = this;
            button.SetTutorial(tutorial);
            StringTableEntry entry = table.GetTable().GetEntry(key);
            if (entry != null)
                if (entry.GetLocalizedString().Contains('+'))
                    button.SetRelationReward(1);
                else
                    button.SetRelationReward(-1);
            else
                playerAnswersTxt[i].gameObject.SetActive(false);

            button.SetNextDialogue(i + 1);

            if (i == 0) {
                StartCoroutine(WaitForGamepad(button.gameObject));
            }
        }
    }

    public void SetTutorialAnswer(int id) {
        npcSpeechTxt.enabled = false;
        npcSpeechTxt.SetKey("Speech" + (tutorialId - 1) + "." + (id + 1));
        npcSpeechTxt.enabled = true;

        playerAnswersTxt[0].enabled = false;
        playerAnswersTxt[0].SetKey("Answer");
        playerAnswersTxt[0].enabled = true;

        DialogueButton button = playerAnswersTxt[0].GetComponent<DialogueButton>();
        button.SetNextDialogue(0);

        StartCoroutine(WaitForGamepad(button.gameObject));
    }

    public void SetAnswer(int id, int relation) {
        npcSpeechTxt.enabled = false;
        npcSpeechTxt.SetKey("Speech" + idDialogue + "." + (id + 1));
        npcSpeechTxt.enabled = true;

        for (int i = 0; i < playerAnswersTxt.Count; i++)
            playerAnswersTxt[i].gameObject.SetActive(false);

        playerAnswersTxt[0].gameObject.SetActive(true);
        playerAnswersTxt[0].enabled = false;
        playerAnswersTxt[0].SetKey("Answer");
        playerAnswersTxt[0].enabled = true;

        currentRegular?.AddFrienship(relation);

        DialogueButton button = playerAnswersTxt[0].GetComponent<DialogueButton>();
        button.SetNextDialogue(0);
        button.SetRelationReward(0);
        StartCoroutine(WaitForGamepad(button.gameObject));
    }

    private IEnumerator WaitForGamepad(GameObject go) {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(go);
    }

    public void SetDefaultButton() {
        if (gameObject.activeSelf) {
            controller.RegisterCurrentSelectedButton();
            StartCoroutine(WaitForGamepad(playerAnswersTxt[0].GetComponent<DialogueButton>().gameObject));
        }
    }

    private void InvokeButton1(InputAction.CallbackContext ctx) {
        if (playerAnswersButtons[0].transform.parent.gameObject.activeSelf)
            playerAnswersButtons[0].onClick?.Invoke();
    }

    private void InvokeButton2(InputAction.CallbackContext ctx) {
        if (playerAnswersButtons[1].transform.parent.gameObject.activeSelf)
            playerAnswersButtons[1].onClick?.Invoke();
    }

    private void InvokeButton3(InputAction.CallbackContext ctx) {
        if (playerAnswersButtons[2].transform.parent.gameObject.activeSelf)
            playerAnswersButtons[2].onClick?.Invoke();
    }

    public void OnDisable() {
        playerControllerSO.GetPlayerController().playerInput.Dialogue.Disable();
        playerControllerSO.GetPlayerController().playerInput.Dialogue.Dialogue1.performed -= InvokeButton1;
        playerControllerSO.GetPlayerController().playerInput.Dialogue.Dialogue2.performed -= InvokeButton2;
        playerControllerSO.GetPlayerController().playerInput.Dialogue.Dialogue3.performed -= InvokeButton3;
        playerControllerSO.GetPlayerController().EnableInput();
        controller.SetEventSystemToLastButton();

        foreach (LocalizedStringComponent button in playerAnswersTxt)
            if (controller.GetEventSystemCurrentlySelected() == button.gameObject)
                controller.SetEventSystemToStartButton(null);

        Time.timeScale = 1;
        OnDestroyDialoguePanel?.Invoke();
        Addressables.ReleaseInstance(gameObject);
    }
}
