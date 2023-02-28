using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float interactionDistance;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject itemSocket;
    [SerializeField] private GameObject interractPanel;

    [SerializeField] private GameObject recipesBook;
    private bool bookDisplayed = false;


    [Header("UI Interaction")]
    [SerializeField] private GameObject interactionText;
    [SerializeField] private TextMeshProUGUI modulableInteractionText;
    [SerializeField] private TextMeshProUGUI productAmountText;

    private PlayerMovements playerMovements;
    private CinemachineFreeLook cinemachine;
    private LocalizedStringComponent localizedStringComponent;
    private GameObject itemHolded;
    private bool playerInputEnable = true;
    private bool interactableFound = false;
    private List<GameObject> gameObjectSelected;
    private GameObject interactedItem;
    private GameObject childrenWithGlass;
    private LocalizedString localizedString;
    private IntVariable productDisplayedAmount = null;
    private StringVariable productDisplayedName = null;
    //[SerializeField] private LocalizeStringEvent ProductDisplayedString;

    [HideInInspector] public PlayerInput playerInput { get; private set; }

    public Controller GetController() => controller;

    // Start is called before the first frame update
    void Awake() {
        playerControllerSO.SetPlayerController(this);
        playerInput = new PlayerInput();
        playerMovements = GetComponent<PlayerMovements>();
        cinemachine = FindObjectOfType<CinemachineFreeLook>();
        playerInput.Player.DisplayRecipesBook.Enable();
        EnableInput();

        localizedString = productAmountText.GetComponent<LocalizeStringEvent>().StringReference;
        if (!localizedString.TryGetValue("ProductDisplayedAmount", out IVariable value)) {
            productDisplayedAmount = new IntVariable();
            localizedString.Add("ProductDisplayedAmount", productDisplayedAmount);
        }
        else {
            productDisplayedAmount = value as IntVariable;
        }

        if (!localizedString.TryGetValue("ProductDisplayedName", out IVariable valueString)) {
            productDisplayedName = new StringVariable();
            localizedString.Add("ProductDisplayedName", productDisplayedAmount);
        }
        else {
            productDisplayedName = valueString as StringVariable;
        }
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



        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position + Vector3.up / 2, transform.forward, interactionDistance);
        if (hitInfo.Length > 0 && hitInfo[0].collider.tag != "Wall") {
            for (int i = 0; i < hitInfo.Length && !interactableFound; i++) {
                if (hitInfo[i].collider.TryGetComponent(out Interactable interactable) && interactable.gameObject != interactedItem) {
                    if (interactedItem) {
                        ClearOutline();
                    }


                    if (!hitInfo[i].collider.GetComponent<AICustomer>()) {
                        interactedItem = interactable.gameObject;
                        interactedItem.gameObject.layer = LayerMask.NameToLayer("Outline");
                    }
                }
            }
            Debug.DrawRay(transform.position + Vector3.up / 2, transform.forward, Color.red);
        }
        else {
            ClearOutline();
        }

        if (interactedItem != null /*&& interactionText.activeSelf == false*/) {
            if (interactedItem.GetComponent<Shelf>()) {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Shelf");
                interactionText.SetActive(true);
                if (interactedItem.GetComponent<Shelf>().GetItem() != null) {

                    Product productInDisplayedText = interactedItem.GetComponent<Shelf>().GetItem().GetComponent<ProductHolder>().product;

                    productDisplayedAmount.Value = productInDisplayedText.GetAmount();
                    productDisplayedName.Value = productInDisplayedText.GetName();
                    productAmountText.enabled = true;

                }
                else {
                    productAmountText.enabled = false;
                }
            }
            else {
                productAmountText.enabled = false;
            }


            interactionText.SetActive(false);
            if (interactedItem.GetComponent<Workstation>()) {
                //modulableInteractionText.text = "to prepare your product";
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Workstation");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<CraftingStation>() && itemHolded != null) {
                if(itemHolded.name.Contains("Paste"))
                {
                    //modulableInteractionText.text = "to cook your products";
                    modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_CookingStation");
                    interactionText.SetActive(true);
                }
            }
            else if (interactedItem.GetComponent<Sink>()) {
                //modulableInteractionText.text = "to wash the plates";
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Sink");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<Computer>()) {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Computer");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<BuildingMode>()) {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Custom");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<Table>()) {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Table");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<EntranceDoor>()) {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Doors");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<TruckDelivery>())
            {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Truck");
                interactionText.SetActive(true);
            }
            else if (interactedItem.GetComponent<Shelf>())
            {
                modulableInteractionText.GetComponent<LocalizedStringComponent>().SetKey("PlayerInteract_Shelf");
                interactionText.SetActive(true);
            }

            //else {
            //    print("Other object detected");
            //    modulableInteractionText.text = "to interact";
            //    interactionText.SetActive(true);
            //}
        }
        else if (interactedItem == null && interactionText.activeSelf == true) {
            interactionText.SetActive(false);
        }
    }

    private void ClearOutline() {
        if (interactedItem) {
            interactedItem.gameObject.layer = LayerMask.NameToLayer("Customizable");

            //foreach (GameObject item in gameObjectSelected)
            //    item.layer = LayerMask.NameToLayer("Customizable");
            //gameObjectSelected.Clear();
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

    private void DisplayBook(InputAction.CallbackContext context)
    {
        if (bookDisplayed)
        {
            recipesBook.SetActive(false);
            bookDisplayed = false;
        }
        else if(!bookDisplayed)
        {
            recipesBook.SetActive(true);
            bookDisplayed = true;
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
        //playerInUI = true;
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
        playerInput.Player.DisplayRecipesBook.performed += DisplayBook;
    }

    private void OnDestroy() {
        playerInput.Player.Interact.performed -= OnInterract;
        playerInput.Player.Pause.performed -= OnPause;
        playerInput.Player.DisplayRecipesBook.performed -= DisplayBook;
    }
}
