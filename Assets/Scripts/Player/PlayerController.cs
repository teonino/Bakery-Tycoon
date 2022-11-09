using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float interactionDistance;

    private PlayerMovements playerMovements;

    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public GameObject itemHolded;

    // Start is called before the first frame update
    void Awake() {
        playerInput = new PlayerInput();

        EnableInput();
        playerMovements = GetComponent<PlayerMovements>();
    }

    public string GetInput(InputAction action) {
        print(action.controls[0].ToString());
        string[] s = action.controls[0].ToString().Split("/");
        return s[s.Length - 1];
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (playerInput.Player.Move.ReadValue<Vector2>().normalized.magnitude == 1) //Prevent reset rotation
            playerMovements.Move(playerInput.Player.Move.ReadValue<Vector2>());

        Debug.DrawRay(transform.position + Vector3.down / 4, transform.forward * interactionDistance, Color.green);
    }

    public void OnInterract(InputAction.CallbackContext context) {
        if (context.performed) {
            RaycastHit[] hitInfo = Physics.RaycastAll(transform.position + Vector3.down / 4, transform.forward, interactionDistance);
            bool interactableFound = false;
            for (int i = 0; i < hitInfo.Length && !interactableFound; i++) {
                if (hitInfo[i].collider.GetComponent<Interactable>()) {
                    hitInfo[i].collider.GetComponent<Interactable>().Effect();
                    interactableFound = true;
                }
            }
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
}
