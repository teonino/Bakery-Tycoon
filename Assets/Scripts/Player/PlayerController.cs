using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float interactionDistance;
    [SerializeField] private Controller controller;
    [SerializeField] private Animator animator;
    private PlayerMovements playerMovements;
    private CinemachineFreeLook cinemachine;
    private GameObject itemHolded;

    [HideInInspector] public PlayerInput playerInput { get; private set; }


    // Start is called before the first frame update
    void Awake() {
        playerInput = new PlayerInput();
        playerMovements = GetComponent<PlayerMovements>();
        cinemachine = FindObjectOfType<CinemachineFreeLook>();
        EnableInput();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (playerInput.Player.Move.ReadValue<Vector2>().normalized.magnitude == 1) { //Prevent reset rotation
            playerMovements.Move(playerInput.Player.Move.ReadValue<Vector2>());
            animator.SetBool("isWalking", true);
        }
        else
            animator.SetBool("isWalking", false);

        if (!controller.IsGamepad()) {
            if (playerInput.Player.AllowCameraMovement.ReadValue<float>() > 0.1f)
                cinemachine.enabled = true;
            else
                cinemachine.enabled = false;
        } else {
            cinemachine.enabled = true; ;
        }


        Debug.DrawRay(transform.position + Vector3.down / 2, transform.forward * interactionDistance, Color.green);
    }

    public void OnInterract(InputAction.CallbackContext context) {
        if (context.performed) {
            RaycastHit[] hitInfo = Physics.RaycastAll(transform.position + Vector3.down / 2, transform.forward, interactionDistance);
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
        FindObjectOfType<GameManager>().Pause();
        DisableInput();
    }

    public void SetItemHold(GameObject go) => itemHolded = go;
    public GameObject GetItemHold() => itemHolded;
    private void OnEnable() {
        playerInput.Player.Interact.performed += OnInterract;
        playerInput.Player.Pause.performed += OnPause;
    }

    public void EnableInput() {
        playerInput.Player.Enable();
    }

    public void DisableInput() {
        playerInput.Player.Disable();
    }

    public string GetInput(InputAction action) {
        print(action.controls[0].ToString());
        string[] s = action.controls[0].ToString().Split("/");
        return s[s.Length - 1];
    }
}
