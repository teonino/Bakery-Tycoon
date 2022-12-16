using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float interactionDistance;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject itemSocket;
    [SerializeField] private GameObject interractPanel;

    private PlayerMovements playerMovements;
    private CinemachineFreeLook cinemachine;
    private GameObject itemHolded;
    private bool playerInUI = false;
    private bool playerInputEnable = true;

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

        //Camera Movement
        if (!FindObjectOfType<CameraSwitch>().switchingCamera) {
            if (!playerInUI) {
                if (!controller.IsGamepad()) {
                    if (playerInput.Player.AllowCameraMovement.ReadValue<float>() > 0.1f)
                        cinemachine.enabled = true;
                    else if (cinemachine.enabled == true)
                        cinemachine.enabled = false;
                }
                else
                    cinemachine.enabled = true; ;
            }
            else
                cinemachine.enabled = false;
        }
    }

    public void OnInterract(InputAction.CallbackContext context) {
        if (context.performed) {
            RaycastHit[] hitInfo = Physics.RaycastAll(transform.position + Vector3.up / 2, transform.forward, interactionDistance);
            bool interactableFound = false;
            for (int i = 0; i < hitInfo.Length && !interactableFound; i++) {
                if (hitInfo[i].collider.GetComponent<Interactable>()) {
                    hitInfo[i].collider.GetComponent<Interactable>().Effect();
                    interactableFound = true;
                }
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
        playerInUI = false;
        playerInputEnable = true;
        controller.InitInputType(this);
    }

    public void DisableInput() {
        playerInUI = true;
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
