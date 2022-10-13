using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Workstation : Interactable {
    [SerializeField] private GameObject workplacePanel;

    private List<Product> products;

    private new void Awake() {
        base.Awake();
        
    }

    private void Start() {
        playerController.playerInput.Workplace.Quit.performed += Quit;
    }

    public override void Effect() {
        if (!playerController.itemHolded) {
            playerController.DisableInput();
            workplacePanel.SetActive(true);
            playerController.playerInput.Workplace.Enable();
        }
    }

    public void Quit(InputAction.CallbackContext context) {
        if (context.performed) {
            WorkstationManager manager = workplacePanel.GetComponent<WorkstationManager>();
            if (manager.product) {
                manager.ResetManager();
            }
            playerController.playerInput.Workplace.Disable();
            playerController.EnableInput();
            workplacePanel.SetActive(false);
        }
    }

    public void CloseWorkplace(GameObject go) {
        Transform arm = playerController.gameObject.transform.GetChild(0);

        playerController.itemHolded = go;
        playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
        playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
        playerController.EnableInput();
        workplacePanel.SetActive(false);
    }
}
