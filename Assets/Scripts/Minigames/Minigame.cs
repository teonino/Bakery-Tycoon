using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public abstract class Minigame : MonoBehaviour {

    [SerializeField] protected CraftingStationType craftingStationRequired;
    [SerializeField] protected float minTime;
    [SerializeField] protected float maxTime;
    [SerializeField] protected float step;
    //public AssetReference minigameAsset;
    protected WorkstationManager workplacePanel;
    protected GameManager gameManager;
    protected PlayerController playerController;
    protected float launchTime;
    protected float endTime;

    // Start is called before the first frame update
    protected void Start() {
        gameManager = FindObjectOfType<GameManager>();
        workplacePanel = transform.parent.gameObject.GetComponent<WorkstationManager>();
        playerController = FindObjectOfType<PlayerController>();
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

        if (go != null)
            go.GetComponent<CraftingStation>().AddDirt();
    }

    protected string GetControl(InputAction action, int index = 0) {
        string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[index])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        foreach (char c in inputName)
            inputName = inputName.Replace(" ", string.Empty);

        if (gameManager.IsGamepad())
            return Gamepad.current[inputName].displayName;
        else
            return Keyboard.current[inputName].displayName;
    }

    public abstract void EnableInputs();
    public abstract void DisableInputs();
}
