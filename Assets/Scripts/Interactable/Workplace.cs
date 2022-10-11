using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : Interactable {
    [SerializeField] private GameObject workplacePanel;

    private List<Product> products;

    private new void Awake() {
        base.Awake();
        //Load all products
    }

    public override void Effect() {
        playerController.DisableInput();
        workplacePanel.SetActive(true);
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
