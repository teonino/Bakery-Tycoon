using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Day", menuName = "Data/Day")]
public class Day : ScriptableObject
{
    [SerializeField] private DayTime dayTime;
    [SerializeField] private int dayCount;
    [SerializeField] private int morningDuration;
    [SerializeField] private int dayDuration;
    private int timeElapsed;

    private void OnEnable() {
        dayTime = DayTime.Morning;
        dayCount = 1;
        timeElapsed = 0;
    }

    private Action UpdateUI;
    public DayTime GetDayTime() => dayTime;
    public int GetDayCount() => dayCount;
    public int GetMorningDuration() => morningDuration;
    public int GetDayDuration() => dayDuration;
    public int GetTimeElapsed() => timeElapsed;
    public void AddTimeElpased(int time) => timeElapsed += time;
    public void SetUpdateUI(Action action) => UpdateUI = action;
    public void NextDayPhase() {
        dayTime++;
        UpdateUI?.Invoke();
    }
}
