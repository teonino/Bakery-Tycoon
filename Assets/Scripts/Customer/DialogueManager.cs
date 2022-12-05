using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI npcSpeechTxt;
    [SerializeField] List<TextMeshProUGUI> playerAnswersTxt;
    [SerializeField] Controller controller;

    private Dialogue dialogue;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        dialogue = new Dialogue();
    }

    public void GetDialogues(int id) {
        try {
            StreamReader s = new StreamReader("Assets\\Dialogues\\classeur" + id + ".csv");

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

        SetDialogue(dialogue);
    }

    public void SetDialogue(Dialogue dialogue) {
        for (int i = 0; i < playerAnswersTxt.Count; i++)
            playerAnswersTxt[i].gameObject.SetActive(false);

        npcSpeechTxt.SetText(dialogue.npcSpeech);

        for (int i = 0; i < dialogue.answers.Count; i++) {
            DialogueButton button = playerAnswersTxt[i].GetComponent<DialogueButton>();

            playerAnswersTxt[i].SetText(dialogue.answers[i].answerText);
            playerAnswersTxt[i].gameObject.SetActive(true);
            button.dialogueManager = this;
            button.SetNextDialogue(dialogue.answers[i].nextDialogue);
            button.SetRelationReward(dialogue.answers[i].relation);

            if (i == 0 && controller.GetInputType() == InputType.Gamepad) {
                gameManager.SetEventSystemToStartButton(button.gameObject);
            }
        }
    }

    public void Destroy() {
        gameManager.GetPlayerController().EnableInput();
        Time.timeScale = 1;
        Addressables.ReleaseInstance(gameObject);
    }
}
