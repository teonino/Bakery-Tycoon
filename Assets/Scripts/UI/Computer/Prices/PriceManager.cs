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

    private GameManager gameManager;
    private PlayerController playerController;
    private InputFieldControllerManager inputFieldControllerManager;
    private GameObject content;
    private List<GameObject> productButtonList;
    private List<GameObject> productRackList;
    private int nbButton = 0;
    private int lenght;

    // Start is called before the first frame update
    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        inputFieldControllerManager = FindObjectOfType<InputFieldControllerManager>();
        content = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        productRackList = new List<GameObject>();
        productButtonList = new List<GameObject>();

        lenght = gameManager.GetProductsLenght();
        playerController = gameManager.GetPlayerController();
    }

    private void Start() {
        //Manage Inputs
        playerController.DisableInput();
        playerController.playerInput.UI.Enable();
        playerController.playerInput.UI.Quit.performed += Quit;

        for (int i = 0; i < lenght; i++) {
            productButtonAsset.InstantiateAsync().Completed += (go) => {
                //go.Result.GetComponent<DeliveryButton>().deliveryManager = this;
                go.Result.GetComponent<PriceButton>().SetProduct(gameManager.GetProductList()[nbButton]);
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
                Navigation navInputField = productButtonList[i].GetComponentInChildren<TMP_InputField>().navigation;

                navButton.mode = navInputField.mode = Navigation.Mode.Explicit;

                if(i < 4) 
                    navInputField.selectOnUp = priceButtonPanel.GetComponent<Button>();

                if (i - 5 >= 0)
                    navInputField.selectOnUp = productButtonList[i - 5].GetComponentInChildren<Button>();
                if (i + 5 < lenght)
                    navButton.selectOnDown = productButtonList[i + 5].GetComponentInChildren<TMP_InputField>();
                if (i + 1 < lenght) {
                    navButton.selectOnRight = productButtonList[i + 1].GetComponentInChildren<Button>();
                    navInputField.selectOnRight = productButtonList[i + 1].GetComponentInChildren<TMP_InputField>();
                }
                if (i - 1 >= 0) {
                    navButton.selectOnLeft = productButtonList[i - 1].GetComponentInChildren<Button>();
                    navInputField.selectOnLeft = productButtonList[i - 1].GetComponentInChildren<TMP_InputField>();
                }

                productButtonList[i].GetComponentInChildren<Button>().navigation = navButton;
                productButtonList[i].GetComponentInChildren<TMP_InputField>().navigation = navInputField;

                //Set OnSelect to trigger movement cooldown
                productButtonList[i].GetComponentInChildren<TMP_InputField>().onSelect.AddListener(delegate { inputFieldControllerManager.OnSelection(); });
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

        Reset();

        computerPanel.SetActive(false);
    }
}
