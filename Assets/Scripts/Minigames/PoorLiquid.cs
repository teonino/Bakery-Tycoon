using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoorLiquid : Minigame {
    [SerializeField] protected float timeAimed = 3;
    [SerializeField] protected float minValue = 0;
    [SerializeField] protected float maxValue = 0;
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image sliderFiller;
    [SerializeField] protected GameObject validZone;

    new void Start() {
        base.Start();
        slider.maxValue = timeAimed;

        validZone.GetComponent<RectTransform>().anchorMin = new Vector2(minValue, 0);
        validZone.GetComponent<RectTransform>().anchorMax = new Vector2(maxValue, 1);
        validZone.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        validZone.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        string inputName = GetControl(playerController.playerInput.PoorLiquid.PoorLiquidAction);
        text.SetText("Hold " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }


    private void FixedUpdate() {
        if (playerController.playerInput.PoorLiquid.PoorLiquidAction.ReadValue<float>() > 0)
            slider.value += Time.deltaTime;
        slider.value -= Time.deltaTime / 3;

        if (CheckInZone()) {
            End();
        }

        if(slider.value == 1) {
            print("Overflowing");
        }
    }

    protected bool CheckInZone() {
        if (slider.value >= minValue && slider.value <= maxValue) {
            sliderFiller.color = Color.green;
            return true;
        }
        else {
            sliderFiller.color = Color.blue;
            return false;
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.PoorLiquid.Enable();
    }

    public override void DisableInputs() {
        playerController.playerInput.PoorLiquid.Disable();
    }
}


