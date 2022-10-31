using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] float interactionDistance;
    [SerializeField] private InputType inputType;

    private PlayerMovements playerMovements;
    public PlayerInput playerInput { get; private set; }
    public GameObject itemHolded;

    // Start is called before the first frame update
    void Awake() {
        playerInput = new PlayerInput();
        if (inputType == InputType.Gamepad) {
            playerInput.devices = new InputDevice[] { Gamepad.all[0] };
            playerInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
        } else {
            playerInput.devices = new InputDevice[] { Keyboard.current, Mouse.current };
            playerInput.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
        }

        EnableInput();
        playerMovements = GetComponent<PlayerMovements>();
    }

    public InputType GetInputType() => inputType;
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
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.down / 4, transform.forward, out hitInfo, interactionDistance) && hitInfo.collider.GetComponent<Interactable>())
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
}
