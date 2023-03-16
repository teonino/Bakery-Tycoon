using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartingPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayTxt;
    [SerializeField] private TextMeshProUGUI customerTxt;
    [SerializeField] private TextMeshProUGUI questTxt;
    [SerializeField] private GameObject button;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListQuest quests;
    [SerializeField] private Day day;
    [SerializeField] private CustomersSO customer;
    [SerializeField] private Controller controller;

    // Start is called before the first frame update
    void Start() {
        if (controller.IsGamepad())
            controller.SetEventSystemToStartButton(button);

        quests.ResetValues();

        Time.timeScale = 0;
        dayTxt.text = $"Day {day.GetDayCount()}";

        questTxt.text = "";
        foreach (Quest quest in quests.GetDailyQuests()) {
            questTxt.text += $"{quest.GetTitle()} \n";
        }
    }

    public void StartDay() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
