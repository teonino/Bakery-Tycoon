using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Minigame : MonoBehaviour {

    [SerializeField] protected CraftingStationType craftingStationRequired;
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
        workplacePanel.MinigameComplete(Random.Range(1, 101));
    }

    protected float GetTimer() => endTime - launchTime; //Return time taken to complete the minigame

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

    public abstract void EnableInputs();
    public abstract void DisableInputs();
}
