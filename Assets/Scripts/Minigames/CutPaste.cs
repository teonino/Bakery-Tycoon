using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutPaste : Minigame {
    [SerializeField] private int aimedValue = 3;
    [SerializeField] private float sizeZone = 0.3f;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;
    [SerializeField] private Image sliderFiller;
    [SerializeField] private int sliderValueChange = 1; //Influence the speed of the slider
    [SerializeField] private GameObject validZone; //Influence the speed of the slider

    private int currentValue = 0;
    private float minValue;
    private float maxValue;

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        SetRandomValidZonePosition();

        InputAction action = playerController.playerInput.CutPaste.CutPasteAction;
        string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[0])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        text.SetText("Tap " + char.ToUpper(inputName[0]) + inputName.Substring(1) +" in timing");
    }

    // Update is called once per frame
    void Update() {
        if (slider.value == slider.maxValue || slider.value == slider.minValue)  
            sliderValueChange *= -1;

        if (slider.value >= minValue && slider.value <= maxValue) {
            sliderFiller.color = Color.green;
        }
        else {
            sliderFiller.color = Color.blue;
        }

        slider.value += Time.deltaTime * sliderValueChange;
    }

    void SetRandomValidZonePosition() {
        minValue = Random.Range(0, 0.8f);
        maxValue = minValue + sizeZone;
        validZone.GetComponent<RectTransform>().anchorMin = new Vector2(minValue, 0);
        validZone.GetComponent<RectTransform>().anchorMax = new Vector2(maxValue, 1);
        validZone.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        validZone.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }
    private void CutPasteAction(InputAction.CallbackContext context) {
        if (context.performed) {
            if (slider.value > minValue && slider.value < maxValue) {
                SetRandomValidZonePosition();
                currentValue++;
            }

            if (currentValue >= aimedValue)
                End();
        }
    }

    public override void DisableInputs() {
        playerController.playerInput.CutPaste.CutPasteAction.performed -= CutPasteAction;
        playerController.playerInput.CutPaste.Disable();
    }

    public override void EnableInputs() {
        playerController.playerInput.CutPaste.Enable();
        playerController.playerInput.CutPaste.CutPasteAction.performed += CutPasteAction;
    }
}
