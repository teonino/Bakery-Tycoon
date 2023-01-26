using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Data/Tutorial")]
public class Tutorial : ScriptableObject {
    [SerializeField] private bool tutorial;

    public Action action;

    private void OnEnable() => tutorial = false;
    
    public void SetTutorial(bool value) => tutorial = value;
    public bool GetTutorial() => tutorial;

    public void Invoke() => action?.Invoke();
}
