using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceDoor : Interactable
{
    [SerializeField] Day day;

    public override void Effect() {
        if (day.GetDayTime() == DayTime.Evening) {
            day.OnNewDay();
            SceneManager.LoadScene("FirstBakery_New");
        }
    }
}
