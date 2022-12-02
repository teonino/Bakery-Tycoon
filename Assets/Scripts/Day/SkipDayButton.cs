using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipDayButton : MonoBehaviour
{
    [SerializeField] Day day;

    public void DisplayButton() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SkipDay() {
        day.NextDayPhase();
        SceneManager.LoadScene(0);
    }
}
