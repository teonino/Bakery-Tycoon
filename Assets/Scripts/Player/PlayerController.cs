using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] float interactionDistance;
    public PlayerInput playerInput { get; private set; }
    private PlayerMovements playerMovements;

    public GameObject itemHolded;

    // Start is called before the first frame update
    void Awake() {
        playerInput = new PlayerInput();
        playerMovements = GetComponent<PlayerMovements>();
        EnableInput();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (playerInput.Player.Move.ReadValue<Vector2>().normalized.magnitude == 1) //Prevent reset rotation
            playerMovements.Move(playerInput.Player.Move.ReadValue<Vector2>());
        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.green);
    }

    public void OnInterract(InputAction.CallbackContext context) {
        if (context.performed) {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, interactionDistance) && hitInfo.collider.GetComponent<Interactable>())
                hitInfo.collider.GetComponent<Interactable>().Effect();
        }
    }

    public void EnableInput() {
        playerInput.Player.Enable();
    }

    public void DisableInput() {
        playerInput.Player.Disable();
    }
    private void OnEnable() {
        playerInput.Player.Interact.performed += OnInterract;
    }

    private void OnDisable() {
        playerInput.Player.Interact.canceled -= OnInterract;
    }
}
