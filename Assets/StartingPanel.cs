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

        //for (int i = 0; i < nbQuest; i++) {
        //    int rng = Random.Range(0, 1);

        //    switch (rng) {
        //        case 0: 
        //            quests.GetQuestList().Add(new CreateQuest(products.GetRandomProduct(), 2));
        //            break;
        //    }
        //}

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
