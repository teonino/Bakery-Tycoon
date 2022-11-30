using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SwitchController : MonoBehaviour {
    private GameManager gameManager;
    public TextMeshProUGUI textButton;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SwitchCurrentController() {
        if (gameManager.IsGamepad()) {
            if (InputSystem.devices.Count > 0) {
                gameManager.GetPlayerController().playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
                gameManager.GetPlayerController().playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
                gameManager.SetInputType(InputType.KeyboardMouse);
                textButton.SetText("Keyboard & Mouse");
            }
        }
        else {
            if (Gamepad.all.Count > 0) {
                gameManager.GetPlayerController().playerInput.devices = new InputDevice[] { Gamepad.all[0] };
                gameManager.GetPlayerController().playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
                gameManager.SetInputType(InputType.Gamepad);
                textButton.SetText("Gamepad");
            }
        }
    } 
}
