using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class WorkstationButton : MonoBehaviour {
    [SerializeField] private ProductSO product;
    [SerializeField] private GameObject productRequirementPanel;
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI productIngredientsText;
    [SerializeField] private TextMeshProUGUI productCreatedText;

    [HideInInspector] public WorkstationManager workplacePanel;
    [HideInInspector] public bool requirementMet;

    private void Start() {
        productCreatedText.SetText(product.name + " x"+product.nbCreated);
        image.texture = product.image;

        productIngredientsText.SetText("Ingredients :\n");
        for (int i = 0; i < product.ingredients.Count; i++) {
            productIngredientsText.text += "    " + product.ingredients[i].name;
            if (i < product.ingredients.Count - 1) 
                productIngredientsText.text += ",\n";
        }
        productIngredientsText.text += "\nCrafting station :\n    " + product.craftStationRequired.ToString();

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
