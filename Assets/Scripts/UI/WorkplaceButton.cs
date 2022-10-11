using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkplaceButton : MonoBehaviour {
    [SerializeField] private ProductSO product;
    [SerializeField] private GameObject productRequirementPanel;

    private Workplace workplace;

    private void Start() {
        GetComponentInChildren<TextMeshProUGUI>().SetText(product.name);
        GetComponentInChildren<RawImage>().texture = product.image;

        workplace = FindObjectOfType<Workplace>();

        if (!product.CheckRequirement()) {
            GetComponent<Button>().enabled = false; // if requirement are not met, disable button
            productRequirementPanel.SetActive(true);
        }
    }

    public void LaunchMinigames() {

    }

    public void SpawnItem() {
        product.asset.InstantiateAsync().Completed += (go) => {
            workplace.CloseWorkplace(go.Result);
        };
    }
}
