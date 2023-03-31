using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipDayButton : MonoBehaviour
{
    [SerializeField] ListRegular list;

    public void DisplayButton() {
        //gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SkipDay() {
        list.AddFriendship(1);
    }
}
