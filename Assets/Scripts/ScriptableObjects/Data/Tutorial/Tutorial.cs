using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Data/Tutorial")]
public class Tutorial : ScriptableObject {
    [SerializeField] private bool tutorial;

    private bool workstation = false;
    private bool AddIngredient = false;

    public Action action;

    private void OnEnable() {
        tutorial = false;
        workstation = AddIngredient = false;
    }
    public void UnlockWorkstation() => workstation = true;
    public bool CanInterractWorkstation() => workstation;
    public void UnlockAddIngredient() => AddIngredient = true;
    public bool CanAddIngredient() => AddIngredient;

    public void SetTutorial(bool value) => tutorial = value;
    public bool GetTutorial() => tutorial;

    public void Invoke() => action?.Invoke();
}
