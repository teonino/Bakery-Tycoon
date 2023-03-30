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

    private List<QuestContainer> questsTxt;

    void Start() {
        questsTxt = new List<QuestContainer>();
        SetQuests();
    }

    private void SetQuests() {
        quests.ResetValues();

        Time.timeScale = 0;
        dayTxt.text = $"Day {day.GetDayCount()}";

        foreach (Quest quest in quests.GetDailyQuests()) {
            questTxtAsset.InstantiateAsync(vLayout.transform).Completed += (go) => {
                QuestContainer questContainer = go.Result.GetComponent<QuestContainer>();

                questContainer.GetTitle().text = $"{quest.GetTitle()} \n";
                questContainer.GetNumber().text = $"{quest.GetCurrentAmount()} / {quest.GetObjective()}\n";
                questsTxt.Add(questContainer);
            };
        }
    }

    public void UpdateUI() {
        for (int i = 0; i < questsTxt.Count; i++) {
            questsTxt[i].GetTitle().text = $"{quests.GetDailyQuests()[i].GetTitle()} \n";
            questsTxt[i].GetNumber().text = $"{quests.GetDailyQuests()[i].GetCurrentAmount()} / {quests.GetDailyQuests()[i].GetObjective()}\n";
        }
    }
}
