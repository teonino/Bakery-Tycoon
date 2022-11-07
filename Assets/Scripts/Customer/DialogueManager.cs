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

    private string npcSpeech;
    private List<ValueTuple<string, int, int>> playerAnswers; //string = player answer / int = next dialogue id / int = relation earned
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetDialogue(int id) {
        for(int i = 0; i < playerAnswersTxt.Count; i++)
            playerAnswersTxt[i].gameObject.SetActive(false);

        playerAnswers = new List<ValueTuple<string, int, int>>();
        try {
            StreamReader s = new StreamReader("Assets\\Dialogues\\classeur" + id + ".csv");
            while (!s.EndOfStream) {
                string line = s.ReadLine();
                string[] values = line.Split(";");
                npcSpeech = values[0]; //Get NPC sppeech

                for (int i = 1; i < values.Length; i++) {
                    string[] answers = values[i].Split("/");
                    for (int j = 0; j < answers.Length; j += 3) {
                        playerAnswers.Add(new ValueTuple<string, int, int>(answers[j], int.Parse(answers[j + 1]), int.Parse(answers[j + 2])));
                    }
                }
            }
            s.Close();
        }
        catch (Exception e) {
            print("Error, Dialoge Manager... " + e.Message);
        }

        npcSpeechTxt.SetText(npcSpeech);

        for (int i = 0; i < playerAnswers.Count; i++) {
            DialogueButton button = playerAnswersTxt[i].GetComponent<DialogueButton>();

            playerAnswersTxt[i].SetText(playerAnswers[i].Item1);
            playerAnswersTxt[i].gameObject.SetActive(true);
            button.dialogueManager = this;
            button.SetNextDialogueID(playerAnswers[i].Item2);
            button.SetRelationReward(playerAnswers[i].Item3);

            if (i == 0 && gameManager.GetInputType() == InputType.Gamepad) {
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
