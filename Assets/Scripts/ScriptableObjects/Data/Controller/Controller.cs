using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Controller", menuName = "Data/Controller")]
public class Controller : ScriptableObject {
    [SerializeField] private InputType inputType;

    private PlayerController playerController;
    private GameObject lastButton;

    public void InitInputType(PlayerController playerController) {
        if (!this.playerController)
            this.playerController = playerController;

        //Set Input Device
        if (inputType == InputType.Gamepad && Gamepad.all.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Gamepad.all[0] };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
        }
        else if (inputType == InputType.KeyboardMouse && InputSystem.devices.Count > 0 && InputSystem.devices.Count > 0) {
            playerController.playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
            playerController.playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
        }
    }

    public InputType GetInputType() => inputType;
    public void SetInputType(InputType value) {
        inputType = value;
        InitInputType(playerController);
    }
    public bool IsGamepad() => inputType == InputType.Gamepad;

    public void SetEventSystemToStartButton(GameObject startButton) {
        if (EventSystem.current) {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }

    public void RegisterCurrentSelectedButton() {
        lastButton = EventSystem.current.currentSelectedGameObject;
    }

    public void SetEventSystemToLastButton() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButton);
    }

    public GameObject GetEventSystemCurrentlySelected() => EventSystem.current.currentSelectedGameObject;
}
