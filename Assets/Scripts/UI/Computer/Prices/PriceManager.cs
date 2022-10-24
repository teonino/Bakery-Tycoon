using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PriceManager : MonoBehaviour {
    [SerializeField] private AssetReference productButtonAsset;
    [SerializeField] private AssetReference productRackAsset;
    [SerializeField] private GameObject computerPanel;

    private GameManager gameManager;
    private GameObject content;
    private List<GameObject> productButtonList;
    private List<GameObject> productRackList;
    private int nbButton = 0;
    private int lenght;

    // Start is called before the first frame update
    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        content = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        productRackList = new List<GameObject>();
        productButtonList = new List<GameObject>();

        lenght = gameManager.GetLenghtProducts();
    }

    private void OnEnable() {
        //Manage Inputs
        gameManager.playerController.DisableInput();
        gameManager.playerController.playerInput.UI.Enable();
        gameManager.playerController.playerInput.UI.Quit.performed += Quit;

        for (int i = 0; i < lenght; i++) {
            productButtonAsset.InstantiateAsync().Completed += (go) => {
                //go.Result.GetComponent<DeliveryButton>().deliveryManager = this;
                go.Result.GetComponent<PriceButton>().SetProduct(gameManager.productsList[nbButton]);
                productButtonList.Add(go.Result);
                nbButton++;
                SetupButtons();
            };

            if (i % 4 == 0) {
                productRackAsset.InstantiateAsync(content.transform).Completed += (go) => {
                    productRackList.Add(go.Result);
                    SetupButtons();
                };
            }
        }
    }

    private void SetupButtons() {
        if (productRackList.Count > 0 && nbButton == lenght) {
            int maxButtonInRack = (int) Math.Floor(content.GetComponent<RectTransform>().rect.x / productButtonList[0].GetComponent<RectTransform>().rect.x) - 1;
            for (int i = 0; i < lenght; i++) {
                productButtonList[i].transform.SetParent(productRackList[i / maxButtonInRack].transform);
                productButtonList[i].transform.localScale = Vector3.one;
            }
        }
    }
    public void Reset() {
        nbButton = 0;

        foreach (GameObject go in productButtonList)
            Addressables.ReleaseInstance(go);
        productButtonList.Clear();

        foreach (GameObject go in productRackList)
            Addressables.ReleaseInstance(go);
        productRackList.Clear();
    }

    public void Quit(InputAction.CallbackContext context) {
        gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
        gameManager.playerController.playerInput.UI.Disable();
        gameManager.playerController.EnableInput();

        Reset();

        computerPanel.SetActive(false);
    }
}
