using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Data/Tutorial")]
public class Tutorial : ScriptableObject {
    [SerializeField] private bool tutorial;

    public void SetTutorial(bool value) => tutorial = value;
    public bool GetTutorial() => tutorial;
}
