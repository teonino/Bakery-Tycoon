using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComputerManager : MonoBehaviour
{
    [SerializeField] private GameObject startButton;

    [HideInInspector] public GameObject lastButton;

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void RegisterCurrentSelectedButton() {
        lastButton = EventSystem.current.currentSelectedGameObject;
    }

    public void SetEventSystemToLastButton() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButton);
    }
}
