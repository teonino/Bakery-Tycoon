using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Data/Tutorial")]
public class Tutorial : Data {
    [SerializeField] private bool tutorial;

    private bool workstation = false;
    private bool AddIngredient = false;

    public Action action;

    public override void ResetValues() {
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
