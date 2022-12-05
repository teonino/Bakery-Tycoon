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
    [SerializeField] private TextMeshProUGUI feedbackText;

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
        inputType = controller.GetInputType();

        if (inputType == InputType.Gamepad) {
            string inputName = GetControl(playerController.playerInput.RollPaste.RollPasteAction);
            text.SetText("Rotate your " + inputName);
            feedbackText.SetText("Spin remaining : " + nbSpinRequired);
        }
        else {
            InputAction action = playerController.playerInput.RollPaste.RollPasteAction;
            string inputName = GetControl(playerController.playerInput.RollPaste.RollPasteAction, 0);
            string inputNameTwo = GetControl(playerController.playerInput.RollPaste.RollPasteAction, 1);
            text.SetText("Spam alternavely : " + inputName + " & " + inputNameTwo);
            feedbackText.SetText("Remaining : " + nbOscilationAimed);
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
                feedbackText.SetText("Spin remaining : " + (nbSpinRequired - ++nbSpin));
                spinCounter = 0;
            }

            if (nbSpin == nbSpinRequired)
                End();
        }
        else {
            if (joyStickInput != lastJoyStickInput && joyStickInput != Vector2.zero) {
                lastJoyStickInput = joyStickInput;
                feedbackText.SetText("Remaining : " + (nbOscilationAimed - ++nbOscilation));
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
