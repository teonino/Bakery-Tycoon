using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BurnTop : Minigame {
    [SerializeField] private int valuePerTap = 10;
    [SerializeField] private int valueLost = 1;
    [SerializeField] private int aimedValue = 50;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;

    new void Start() {
        base.Start();
        slider.maxValue = aimedValue;

        string inputName = GetControl(playerController.playerInput.BurnTop.BurnTopAction);

        text.SetText("Spam " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }

    void Update() => slider.value -= Time.deltaTime * valueLost;

    private void BurnTopAction(InputAction.CallbackContext context) {
        if (context.performed) {
            slider.value += valuePerTap;
            if (slider.value >= aimedValue) {
                End();
            }
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.BurnTop.Enable();
        playerController.playerInput.BurnTop.BurnTopAction.performed += BurnTopAction;
    }

    public override void DisableInputs() {
        playerController.playerInput.BurnTop.BurnTopAction.performed -= BurnTopAction;
        playerController.playerInput.BurnTop.Disable();
    }
}
