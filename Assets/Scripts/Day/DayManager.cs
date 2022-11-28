using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

    private GameManager gameManager;

    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotationMorning;
    [SerializeField] private int endLightRotation;
    [SerializeField] private int morningDuration;
    [SerializeField] private int dayDuration;

    private float timeElapsed;
    private float duration;
    private Action<DayTime, int> updateDayTimeUI;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        updateDayTimeUI = FindObjectOfType<DayTimeUI>().StartDayTime;
        updateDayTimeUI(gameManager.dayTime, morningDuration);
        duration = morningDuration + dayDuration;
    }

    //void FixedUpdate() {
    //    if (daySystemEnable) {
    //        if (timeElapsed < duration) {
    //            transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / duration));
    //            timeElapsed += Time.deltaTime;

    //            if (timeElapsed > morningDuration && gameManager.dayTime == DayTime.Morning)
    //                Updateday();
    //        }
    //        else if (gameManager.dayTime == DayTime.Day)
    //            Updateday();
    //    }
    //}

    void FixedUpdate() {
        if (daySystemEnable) {
            if (timeElapsed > morningDuration && gameManager.dayTime == DayTime.Morning)
                Updateday();

            timeElapsed += Time.deltaTime;
            transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotationMorning, timeElapsed / duration));
        }
    }


    public void Updateday() {
        gameManager.dayTime++;
        updateDayTimeUI(gameManager.dayTime, 0);
    }

    public void SetRotation(int nbCustomer, int currentCustomer, float time) {
        if (nbCustomer == currentCustomer)
            Updateday();

        startLightRotation = endLightRotation / nbCustomer * currentCustomer;
        if (startLightRotation == 0)
            startLightRotation = endLightRotationMorning;
        endLightRotationMorning = endLightRotation / nbCustomer * (currentCustomer + 1);

        timeElapsed = 0;
        duration = time;
    }

    public int GetDayDuration() => dayDuration;
    public int GetMorningDuration() => morningDuration;
}
