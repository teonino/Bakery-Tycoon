using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public abstract class Minigame : MonoBehaviour {
    [Header("Global parameters")]
    [SerializeField] protected Controller controller;
    [SerializeField] protected CraftingStationType craftingStationRequired;
    [SerializeField] protected float minTime;
    [SerializeField] protected float maxTime;
    [SerializeField] protected float step;
    [Header("Minigame parameters")]

    protected WorkstationManager workplacePanel;
    protected PlayerController playerController;
    protected float launchTime;
    protected float endTime;


    // Start is called before the first frame update
    protected void Start() {
        workplacePanel = FindObjectOfType<WorkstationManager>(true);
        playerController = FindObjectOfType<PlayerController>();
        controller = playerController.GetController();
        launchTime = Time.time;
        EnableInputs();
    }

    protected void End() {
        endTime = Time.time;
        DisableInputs();
        DirtyCraftingStation();
        Addressables.ReleaseInstance(gameObject);
        workplacePanel.MinigameComplete(GetQuality());
    }

    private int GetQuality() {
         float time = endTime - launchTime;

        if (time < minTime) return 100;
        else if (time > maxTime) return 10;
        else return Mathf.RoundToInt((maxTime - minTime) * (maxTime - minTime) * 10 - (time - minTime) / step * (maxTime - minTime));
    }

    protected void DirtyCraftingStation() {
        GameObject go = null;
        switch (craftingStationRequired) {
            case CraftingStationType.Hoven:
                go = GameObject.FindGameObjectWithTag("Hoven");
                break;
            default:
                break;
        }
    }

    protected virtual void Update() {
        if (gameObject.activeSelf && playerController.GetPlayerInputEnabled())
            playerController.DisableInput();
    }

    protected string GetControl(InputAction action, int index = 0) {
        string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[index])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        foreach (char c in inputName)
            inputName = inputName.Replace(" ", string.Empty);

        if (controller.IsGamepad())
            return Gamepad.current[inputName].displayName;
        else
            return Keyboard.current[inputName].displayName;
    }

    public abstract void EnableInputs();
    public abstract void DisableInputs();
}
