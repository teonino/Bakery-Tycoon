using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipDayButton : MonoBehaviour
{
    GameManager gameManager;

    void Start() { 
        gameManager = FindObjectOfType<GameManager>();
    }
    public void DisplayButton() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SkipDay() {
        SceneManager.LoadScene(0);
    }
}
