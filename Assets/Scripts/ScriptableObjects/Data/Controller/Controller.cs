using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Controller", menuName = "Data/Controller")]
public class Controller : ScriptableObject
{
    [SerializeField] private InputType inputType;

    public InputType GetInputType() => inputType;
    public bool IsGamepad() => inputType == InputType.Gamepad;
}
