using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StartingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayTxt;
    [SerializeField] private AssetReference questTxtAsset;
    [SerializeField] private GameObject vLayout;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListQuest quests;
    [SerializeField] private Day day;
    [SerializeField] private Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        quests.ResetValues();

        Time.timeScale = 0;
        dayTxt.text = $"Day {day.GetDayCount()}";

        foreach (Quest quest in quests.GetDailyQuests())
        {
            questTxtAsset.InstantiateAsync(vLayout.transform).Completed += (go) =>
            {
                go.Result.GetComponent<QuestContainer>().GetTitle().text= $"{quest.GetTitle()} \n";
                go.Result.GetComponent<QuestContainer>().GetNumber().text = $"{quest.GetCurrentAmount()} / {quest.GetObjective()}\n";
            };
        }
    }

    public void StartDay()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
