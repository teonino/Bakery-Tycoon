using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StartingPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayTxt;
    [SerializeField] private AssetReference questTxtAsset;
    [SerializeField] private GameObject vLayout;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListQuest quests;
    [SerializeField] private Day day;
    [SerializeField] private Controller controller;
    [SerializeField] private Tutorial tutorial;

    private List<QuestContainer> questsTxt;

    void Start() {
        if (tutorial.GetTutorial())
            gameObject.SetActive(false);
        else {
            questsTxt = new List<QuestContainer>();
            SetQuests();
        }
    }

    private void SetQuests() {
        quests.ResetValues();

        Time.timeScale = 0;
        dayTxt.text = $"Day {day.GetDayCount()}";

        foreach (Quest quest in quests.GetDailyQuests()) {
            questTxtAsset.InstantiateAsync(vLayout.transform).Completed += (go) => {
                QuestContainer questContainer = go.Result.GetComponent<QuestContainer>();

                string tmp;

                questContainer.GetLocalizedString().SetKey(quest.GetKey());

                tmp = questContainer.GetTitle().text;

                if (quest.GetVariable() != "") {
                    tmp += quest.GetVariable();
                    questContainer.GetLocalizedString().enabled = false;
                }
                questContainer.GetTitle().text = tmp;

                questContainer.GetNumber().text = $"{quest.GetCurrentAmount()} / {quest.GetObjective()}\n";
                questsTxt.Add(questContainer);
            };
        }
    }

    public void UpdateUI() {
        if (questsTxt != null) {
            for (int i = 0; i < questsTxt.Count; i++) {
                questsTxt[i].GetNumber().text = $"{quests.GetDailyQuests()[i].GetCurrentAmount()} / {quests.GetDailyQuests()[i].GetObjective()}\n";
            }
        }
    }
}
