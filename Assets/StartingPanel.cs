using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartingPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayTxt;
    [SerializeField] private TextMeshProUGUI customerTxt;
    [SerializeField] private TextMeshProUGUI questTxt;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListQuest quests;
    [SerializeField] private Day day;

    // Start is called before the first frame update
    void Start() {
        quests.ResetValues();

        Time.timeScale = 0;
        dayTxt.text = $"Day {day.GetCurrentDay()}";
        customerTxt.text = $"X Customers expected";

        questTxt.text = "";
        foreach (Quest quest in quests.GetQuestList()) {
            questTxt.text += $"{quest.GetTitle()} \n";
        }
    }

    public void StartDay() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
