using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollPaste : Minigame {
    [Header("Gamepad parameters")]
    [SerializeField] private float nbSpinRequired = 5;
    [SerializeField] private float angleLimit = 30f;
    [SerializeField] private float checkAngleTime = 0.1f;
    [SerializeField] private int spinRows = 10;
    [SerializeField] private TextMeshProUGUI text;

    private Vector2 joyStickInput = Vector2.zero;
    private Vector2 lastJoyStickInput = Vector2.zero;
    private bool spinning = false;
    private bool checkingRotation = false;
    private int spinCounter = 0;
    private int nbSpin = 0;

    [Header("Keyboard parameters")]
    [SerializeField] private int nbOscilationAimed = 10;
    private int nbOscilation = 0;

    private InputType inputType;

    new void Start() {
        base.Start();
        inputType = playerController.GetInputType();

        if (inputType == InputType.Gamepad)
            text.SetText("Rotate your Joystick");
        else {
            InputAction action = playerController.playerInput.RollPaste.RollPasteAction;
            string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[0])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            string inputNameTwo = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[1])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            text.SetText("Spam : " + inputName + " & " + inputNameTwo);
        }
    }

    private void Update() {
        joyStickInput = playerController.playerInput.RollPaste.RollPasteAction.ReadValue<Vector2>();

        if (inputType == InputType.Gamepad) { 
            if (joyStickInput != lastJoyStickInput && !checkingRotation) {
                checkingRotation = true;
                StartCoroutine(SpinDetector());
            }

            if (spinCounter == spinRows)
                spinning = true;
            else
                spinning = false;

            if (spinning) {
                print("Spin : " + ++nbSpin);
                spinCounter = 0;
            }

            if (nbSpin == nbSpinRequired)
                End();
        }
        else {
            if (joyStickInput != lastJoyStickInput && joyStickInput != Vector2.zero) {
                lastJoyStickInput = joyStickInput;
                nbOscilation++;
                print(nbOscilation);
                if (nbOscilation >= nbOscilationAimed)
                    End();
            }
        }
    }

    public override void EnableInputs() => playerController.playerInput.RollPaste.Enable();
    public override void DisableInputs() => playerController.playerInput.RollPaste.Disable();

    IEnumerator SpinDetector() {
        lastJoyStickInput = joyStickInput;

        yield return new WaitForSeconds(checkAngleTime);

        if (Vector2.Angle(lastJoyStickInput, joyStickInput) >= angleLimit) {
            spinCounter++;
            spinCounter = Mathf.Clamp(spinCounter, 0, spinRows);
        }
        else {
            spinCounter = 0;
        }
        checkingRotation = false;
    }
}
