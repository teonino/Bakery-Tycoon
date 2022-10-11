using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkplaceButton : MonoBehaviour
{
    [SerializeField] private ProductSO product;

    private Workplace workplace;

    private void Start() {
        GetComponentInChildren<TextMeshProUGUI>().SetText(product.name);
        GetComponentInChildren<RawImage>().texture = product.image;

        workplace = FindObjectOfType<Workplace>();
    }

    public void LaunchMinigames() {

    }

    public void SpawnItem() {
        product.asset.InstantiateAsync().Completed += (go) => {
            workplace.CloseWorkplace(go.Result);
        };
    }
}
