using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PriceManager : MonoBehaviour {
    [SerializeField] private AssetReference productButtonAsset;
    [SerializeField] private AssetReference productRackAsset;
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private GameObject priceButtonPanel;
    [SerializeField] private ListProduct products;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private InterractQuest interractQuest;

    private PlayerController playerController;
    private InputFieldControllerManager inputFieldControllerManager;
    private GameObject content;
    private List<GameObject> productButtonList;
    private List<GameObject> productRackList;
    private int nbButton = 0;
    private int lenght;

    // Start is called before the first frame update
    void Awake() {
        inputFieldControllerManager = FindObjectOfType<InputFieldControllerManager>();
        content = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        productRackList = new List<GameObject>();
        productButtonList = new List<GameObject>();

        lenght = products.GetProductLenght();
        playerController = playerControllerSO.GetPlayerController();
    }

    private void OnEnable() {
        //Manage Inputs
        playerController.DisableInput();
        playerController.playerInput.UI.Enable();
        playerController.playerInput.UI.Quit.performed += Quit;

        interractQuest.OnInterract();
    }

    private void Start() {
        for (int i = 0; i < lenght; i++) {
            productButtonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<PriceButton>().SetProduct(products.GetProductList()[nbButton]);
                inputFieldControllerManager.listInputField.Add(go.Result.GetComponentInChildren<TMP_InputField>());
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
            int maxButtonInRack = (int)Math.Floor(content.GetComponent<RectTransform>().rect.x / productButtonList[0].GetComponent<RectTransform>().rect.x) - 1;
            for (int i = 0; i < lenght; i++) {
                productButtonList[i].transform.SetParent(productRackList[i / maxButtonInRack].transform);
                productButtonList[i].transform.localScale = Vector3.one;
            }

            for (int i = 0; i < lenght; i++) {
                Navigation navButton = productButtonList[i].GetComponentInChildren<Button>().navigation;

                navButton.mode = Navigation.Mode.Explicit;

                if (i < maxButtonInRack)
                    navButton.selectOnUp = priceButtonPanel.GetComponent<Button>();
                else
                    navButton.selectOnUp = productButtonList[i - maxButtonInRack].GetComponentInChildren<Button>();

                if (i + maxButtonInRack < lenght)
                    navButton.selectOnDown = productButtonList[i + maxButtonInRack].GetComponentInChildren<Button>();
                if (i + 1 < lenght) 
                    navButton.selectOnRight = productButtonList[i + 1].GetComponentInChildren<Button>();
                if (i - 1 >= 0) 
                    navButton.selectOnLeft = productButtonList[i - 1].GetComponentInChildren<Button>();
                

                productButtonList[i].GetComponentInChildren<Button>().navigation = navButton;
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
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();

        computerPanel.SetActive(false);
    }
}
