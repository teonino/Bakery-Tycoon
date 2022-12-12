using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Day", menuName = "Data/Day")]
public class Day : ScriptableObject {
    [SerializeField] private DayTime dayTime;
    [SerializeField] private int dayCount;
    [SerializeField] private int morningDuration;
    [SerializeField] private int dayDuration;
    private int timeElapsed;
    private Action DayTimeChange;
    private Action NewDay;

    private void OnEnable() {
        dayTime = DayTime.Morning;
        timeElapsed = 0;
    }

    public DayTime GetDayTime() => dayTime;
    public int GetDayCount() => dayCount;
    public int GetMorningDuration() => morningDuration;
    public int GetDayDuration() => dayDuration;
    public int GetTimeElapsed() => timeElapsed;
    public void AddTimeElpased(int time) => timeElapsed += time;
    public void AddEventOnDayTimeChange(Action action) => DayTimeChange = action;
    public void AddEventOnNewDay(Action action) => NewDay = action;
    public void OnNextDayPhase() {
        dayTime++;
        DayTimeChange?.Invoke();
    }
    public void OnNewDay() {
        dayCount++;
        dayTime = DayTime.Morning;
        NewDay?.Invoke();
        SceneManager.LoadSceneAsync("FirstBakery");
    }
}
