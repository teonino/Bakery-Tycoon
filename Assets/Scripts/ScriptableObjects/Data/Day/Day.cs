using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Day", menuName = "Data/Day")]
public class Day : Data {
    [SerializeField] private string currentScene;
    [SerializeField] private DayTime dayTime;
    [SerializeField] private int dayCount;

    private int timeElapsed;
    public Action DayTimeChange;
    public Action NewDay;
    
    public override void ResetValues() {
        dayTime = DayTime.Day;
        timeElapsed = 0;
    }

    public DayTime GetDayTime() => dayTime;
    public int GetTimeElapsed() => timeElapsed;
    public int GetDayCount() => dayCount;
    public void AddTimeElpased(int time) => timeElapsed += time;
    public void OnNextDayPhase() {
        dayTime++;
        DayTimeChange?.Invoke();
        Debug.Log("c'est passé par la");
    }
    public void OnNewDay() {
        dayCount++;
        dayTime = DayTime.Day;
        timeElapsed = 0;
        NewDay?.Invoke();
    }
}
