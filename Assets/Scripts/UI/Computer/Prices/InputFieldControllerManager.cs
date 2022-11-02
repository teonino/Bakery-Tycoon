using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputFieldControllerManager : MonoBehaviour {
    [HideInInspector] public List<TMP_InputField> listInputField;
    private float lastMove = 0;

    void Update() {
        foreach (TMP_InputField field in listInputField) {
            if (field.isFocused && lastMove < Time.time - 0.2f) {
                if (Gamepad.current.leftStick.ReadValue().y < -0.5 || Gamepad.current.dpad.ReadValue().y < -0.5)
                    SelectUIElement(field.FindSelectableOnDown());
                else if (Gamepad.current.leftStick.ReadValue().y > 0.5 || Gamepad.current.dpad.ReadValue().y > 0.5)
                    SelectUIElement(field.FindSelectableOnUp());
                else if (Gamepad.current.leftStick.ReadValue().x > 0 || Gamepad.current.dpad.ReadValue().x > 0)
                    SelectUIElement(field.FindSelectableOnRight());
                else if (Gamepad.current.leftStick.ReadValue().x < 0 || Gamepad.current.dpad.ReadValue().x < 0)
                    SelectUIElement(field.FindSelectableOnLeft());
            }
        }
    }

    public void OnSelection() {
        lastMove = Time.time;
        print("selection");
    }

    private void SelectUIElement(Selectable s) {
        if (s) {
            s.Select();
            lastMove = Time.time;
        }
    }
}
