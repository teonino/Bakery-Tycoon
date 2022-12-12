using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimeUI : MonoBehaviour {
    [SerializeField] private Day day;
    private TextMeshProUGUI text;
    private int duration;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
        TmpBuild.instance.day.AddEventOnDayTimeChange(SetDay);
        SetDay();
    }

    private void SetDay() {
        if (TmpBuild.instance.day.GetDayTime() == DayTime.Morning) {
            StartCoroutine(TimeRemaining());
        }
        else {
            text.SetText(GetDay());
        }
    }

    private IEnumerator TimeRemaining() {
        if (TmpBuild.instance.day.GetDayTime() == DayTime.Morning)
            duration = TmpBuild.instance.day.GetMorningDuration();
        else
            duration = TmpBuild.instance.day.GetDayDuration() + TmpBuild.instance.day.GetMorningDuration();

        int timeRemaining = duration - TmpBuild.instance.day.GetTimeElapsed();

        text.SetText(GetDay() + " " + timeRemaining / 60  + ":"); // Display minutes
        if (timeRemaining % 60 < 10)
            text.text += "0" + timeRemaining % 60;
        else
            text.text += timeRemaining % 60;

        yield return new WaitForSeconds(1);
        TmpBuild.instance.day.AddTimeElpased(1);
        if (TmpBuild.instance.day.GetTimeElapsed() > 0 && TmpBuild.instance.day.GetDayTime() != DayTime.Evening)
            StartCoroutine(TimeRemaining());
    }

    public string GetDay() {
        string s = "";
        switch (TmpBuild.instance.day.GetDayTime()) {
            case DayTime.Morning:
                s = "Morning";
                break;
            case DayTime.Day:
                s = "Day";
                break;
            case DayTime.Evening:
                s = "Evening";
                break;
            default:
                break;
        }
        return s;
    }
}
