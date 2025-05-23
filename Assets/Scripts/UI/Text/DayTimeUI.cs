using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimeUI : MonoBehaviour
{
    [SerializeField] private Day day;
    private LocalizedStringComponent text;
    private int duration;

    private void Start()
    {
        text = GetComponent<LocalizedStringComponent>();
        SetDay();
    }

    private void OnEnable()
    {
        day.DayTimeChange += SetDay;
    }

    private void OnDisable()
    {
        day.DayTimeChange -= SetDay;
    }

    private void SetDay()
    {
        text.enabled = false;
        switch (day.GetDayTime())
        {
            case DayTime.Day:
                text.SetKey("Day");
                break;
            case DayTime.Evening:
                text.SetKey("Evening");
                break;
        }
        text.enabled = true;
    }

        //private IEnumerator TimeRemaining() {
        //    duration = day.GetDayDuration();

        //    int timeRemaining = duration - day.GetTimeElapsed();

        //    text.SetText(GetDayTime() + " " + timeRemaining / 60 + ":"); // Display minutes
        //    if (timeRemaining % 60 < 10)
        //        text.text += "0" + timeRemaining % 60;
        //    else
        //        text.text += timeRemaining % 60;

        //    yield return new WaitForSeconds(1);
        //    day.AddTimeElpased(1);
        //    if (day.GetTimeElapsed() > 0 && day.GetDayTime() != DayTime.Evening)
        //        StartCoroutine(TimeRemaining());
        //}

        public string GetDayTime()
        {
            string s = "";
            switch (day.GetDayTime())
            {
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

        public Day GetDay() => day;
    }
