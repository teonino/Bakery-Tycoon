using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float interactionDistance;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject itemSocket;
    [SerializeField] private GameObject interractPanel;

    [Header("Outline")]
    [SerializeField] private List<GameObject> gameObjectSelected;
    [SerializeField] private List<GameObject> ChildGameObjectSelected;
    [SerializeField] private List<LayerMask> ChildLayerSelected;
    [SerializeField] private GameObject interactedItem;
    private GameObject childrenWithGlass;
    [Header("UI Interaction")]
    [SerializeField] private GameObject interactionText;
    [SerializeField] private LocalizedStringComponent modulableInteractionText;
    [SerializeField] private TextMeshProUGUI pressText;

    private PlayerMovements playerMovements;
    private CinemachineFreeLook cinemachine;
    private LocalizedStringComponent localizedStringComponent;
    private GameObject itemHolded;
    private bool playerInputEnable = true;
    bool interactableFound = false;


    [HideInInspector] public PlayerInput playerInput { get; private set; }

    public Controller GetController() => controller;

    // Start is called before the first frame update
    void Awake() {
        playerControllerSO.SetPlayerController(this);
        playerInput = new PlayerInput();
        playerMovements = GetComponent<PlayerMovements>();
        cinemachine = FindObjectOfType<CinemachineFreeLook>();
        EnableInput();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        //Player Movement
        if (playerInput.Player.Move.ReadValue<Vector2>().normalized.magnitude == 1) { //Prevent reset rotation
            playerMovements.Move(playerInput.Player.Move.ReadValue<Vector2>());
            animator.SetBool("isWalking", true);
        }
        else
            animator.SetBool("isWalking", false);

        Debug.DrawRay(transform.position + Vector3.up / 2, transform.forward, Color.red);
        Outline();
    }

    private void Outline() {
        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position + Vector3.up / 2, transform.forward, interactionDistance);
        if (hitInfo.Length > 0 && hitInfo[0].collider.tag != "Wall") {
            for (int i = 0; i < hitInfo.Length; i++) {
                if (hitInfo[i].collider.TryGetComponent(out Interactable interactable)) {
                    if (interactedItem)
                        ClearOutline();

                    if (!hitInfo[i].collider.GetComponent<AICustomer>()) {
                        interactedItem = interactable.gameObject;
                        interactedItem.gameObject.layer = LayerMask.NameToLayer("Outline");
                    }
                }
            }
        }
        else {
            ClearOutline();
        }

        if (interactedItem) {
            if (interactedItem.GetComponent<Shelf>())
                modulableInteractionText.SetKey("PlayerInteract_Shelf");

            else if (interactedItem.GetComponent<Workstation>())
                modulableInteractionText.SetKey("PlayerInteract_Workstation");

            else if (interactedItem.GetComponent<CraftingStation>())
                modulableInteractionText.SetKey("PlayerInteract_CookingStation");

            else if (interactedItem.GetComponent<Sink>())
                modulableInteractionText.SetKey("PlayerInteract_Sink");

            else if (interactedItem.GetComponent<Computer>())
                modulableInteractionText.SetKey("PlayerInteract_Computer");

            else if (interactedItem.GetComponent<BuildingMode>())
                modulableInteractionText.SetKey("PlayerInteract_Custom");

            else if (interactedItem.GetComponent<Table>())
                modulableInteractionText.SetKey("PlayerInteract_Table");

            else if (interactedItem.GetComponent<EntranceDoor>())
                modulableInteractionText.SetKey("PlayerInteract_Doors");


            interactionText.SetActive(false);
            interactionText.SetActive(true);
        }
        else if (!interactedItem && interactionText.activeSelf) {
            interactionText.SetActive(false);
        }
    }

    private void ClearOutline() {
        if (interactedItem) {
            interactedItem.gameObject.layer = LayerMask.NameToLayer("Customizable");
            foreach (GameObject item in gameObjectSelected)
                item.layer = LayerMask.NameToLayer("Customizable");

            gameObjectSelected.Clear();
            interactedItem = null;
        }
    }


    public void OnInterract(InputAction.CallbackContext context) {
        if (context.performed) {
            RaycastHit[] hitInfo = Physics.RaycastAll(transform.position + Vector3.up / 2, transform.forward, interactionDistance);
            if (hitInfo.Length > 0 && hitInfo[0].collider.tag != "Wall") {
                for (int i = 0; i < hitInfo.Length && !interactableFound; i++) {
                    if (hitInfo[i].collider.GetComponent<Interactable>()) {
                        hitInfo[i].collider.GetComponent<Interactable>().Effect();
                        interactableFound = true;
                    }
                }
                interactableFound = false;
            }
        }
    }
    private void OnPause(InputAction.CallbackContext context) {
        FindObjectOfType<PauseManager>(true).gameObject.SetActive(true);
        DisableInput();
    }

    public void SetItemHold(GameObject go) => itemHolded = go;
    public GameObject GetItemHold() => itemHolded;

    public void EnableInput() {
        playerInput.Player.Enable();
        //playerInUI = false;
        playerInputEnable = true;
        controller.InitInputType(this);
    }

    public void DisableInput() {
        playerInputEnable = false;
        playerInput.Player.Disable();
    }

    public string GetInput(InputAction action) {
        print(action.controls[0].ToString());
        string[] s = action.controls[0].ToString().Split("/");
        return s[s.Length - 1];
    }

    public bool GetPlayerInputEnabled() => playerInputEnable;
    public GameObject GetItemSocket() => itemSocket;

    private void OnEnable() {
        playerInput.Player.Interact.performed += OnInterract;
        playerInput.Player.Pause.performed += OnPause;
    }

    private void OnDestroy() {
        playerInput.Player.Interact.performed -= OnInterract;
        playerInput.Player.Pause.performed -= OnPause;
    }
}
