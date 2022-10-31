using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class WorkstationButton : MonoBehaviour {
    [SerializeField] private ProductSO product;
    [SerializeField] private GameObject productRequirementPanel;

    public WorkstationManager workplacePanel;
    public bool requirementMet;

    private void Start() {
        GetComponentInChildren<TextMeshProUGUI>().SetText(product.name);
        GetComponentInChildren<RawImage>().texture = product.image;

        requirementMet = true;

        if (!requirementMet) {
            GetComponent<Button>().enabled = false; // if requirement are not met, disable button
            productRequirementPanel.SetActive(true);
        }
    }

    public void SetOngoingProduct() {
        workplacePanel.SetProduct(product);
    }

    public void SetProduct(ProductSO product) => this.product = product;
}
