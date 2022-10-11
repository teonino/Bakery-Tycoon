using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WorkplacePanel : MonoBehaviour {
    [SerializeField] private AssetReference productButton;

    private List<GameObject> productButtonList;

    private int nbButton = 0;
    private int nbButtonRequired = 4;

    //Create buttons
    private void Start() {
        productButtonList = new List<GameObject>();

        for (int i = 0; i < nbButtonRequired; i++) {
            productButton.InstantiateAsync(transform).Completed += (go) => {
                productButtonList.Add(go.Result);
                SetupButtons();
            };
        }
    }

    //If enough button created, we position them
    private void SetupButtons() {
        if (nbButton == nbButtonRequired - 1) {
            for (int i = 0; i < nbButtonRequired; i++) {
                productButtonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20 + 120 * i, -20, 0);
            }
        }
        nbButton++;
    }
}
