using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SwitchController : MonoBehaviour {
    public TextMeshProUGUI textButton;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;

    private void Start() {
        if (Gamepad.all.Count > 0) {
            controller.SetInputType(InputType.Gamepad);
            textButton.SetText("Gamepad");
        }
        else if (InputSystem.devices.Count > 0) {
            controller.SetInputType(InputType.KeyboardMouse);
            textButton.SetText("Keyboard & Mouse");
        }
    }

    public void SwitchCurrentController() {
        if (controller.IsGamepad()) {
            if (InputSystem.devices.Count > 0) {
                playerControllerSO.GetPlayerController().playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
                playerControllerSO.GetPlayerController().playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
                controller.SetInputType(InputType.KeyboardMouse);
                textButton.SetText("Keyboard & Mouse");
            }
        }
        else {
            if (Gamepad.all.Count > 0) {
                playerControllerSO.GetPlayerController().playerInput.devices = new InputDevice[] { Gamepad.all[0] };
                playerControllerSO.GetPlayerController().playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
                controller.SetInputType(InputType.Gamepad);
                textButton.SetText("Gamepad");
            }
        }
    }
}
