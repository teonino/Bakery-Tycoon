using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class WorkstationProductButton : MonoBehaviour {
    [SerializeField] private ProductSO product;
    [SerializeField] private GameObject productRequirementPanel;
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI productIngredientsText;
    [SerializeField] private TextMeshProUGUI productCreatedText;

    [HideInInspector] public WorkstationManager workplacePanel;
    private bool requirementMet;

    private void Start() {
        productCreatedText.SetText(product.name + " x" + product.nbCreated);
        image.texture = product.image;

        productIngredientsText.SetText("Ingredients :\n");
        for (int i = 0; i < product.ingredients.Count; i++) {
            productIngredientsText.text += "    " + product.ingredients[i].name;
            if (i < product.ingredients.Count - 1)
                productIngredientsText.text += ",\n";
        }
        productIngredientsText.text += "\nCrafting station :\n    " + product.craftStationRequired.ToString();

        CheckRequirement();
    }

    public void SetRequirement(bool requirementMet) {
        this.requirementMet = requirementMet;
        CheckRequirement();
    }

    private void CheckRequirement() {
        if (!requirementMet) {
            GetComponent<Button>().enabled = false;
            productRequirementPanel.SetActive(true);
        } else {
            GetComponent<Button>().enabled = true;
            productRequirementPanel.SetActive(false);
        }
    }

    public void SetOngoingProduct() {
        //workplacePanel.SetProduct(product);
    }
    public ProductSO GetProduct() => product;

    public void SetProduct(ProductSO product) => this.product = product;
}
