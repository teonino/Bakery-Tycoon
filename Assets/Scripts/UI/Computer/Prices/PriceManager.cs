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
    [SerializeField] private Controller controller;
    [SerializeField] private InterractQuest interractQuest;
    [SerializeField] private GameObject scroll;
    [SerializeField] private int scrollSpeed;

    private PlayerController playerController;
    private InputFieldControllerManager inputFieldControllerManager;
    private List<GameObject> productButtonList;
    private List<GameObject> productRackList;
    private int nbButton = 0;
    private int lenght;
    private int maxButtonInRack;
    private RectTransform scrollRectTransform;

    // Start is called before the first frame update
    void Awake() {
        inputFieldControllerManager = FindObjectOfType<InputFieldControllerManager>();
        scrollRectTransform = scroll.GetComponent<RectTransform>();
        productRackList = new List<GameObject>();
        productButtonList = new List<GameObject>();
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
        lenght = products.GetProductLenght();

        for (int i = 0; i < lenght; i++) {
            productButtonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<PriceButton>().SetProduct(products.GetProductList()[nbButton]);
                inputFieldControllerManager.listInputField.Add(go.Result.GetComponentInChildren<TMP_InputField>());
                productButtonList.Add(go.Result);
                nbButton++;
                SetupRacks();
            };
        }
    }

    private void SetupRacks() {
        if (productButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scroll.GetComponent<RectTransform>().rect.width / productButtonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < productButtonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    productRackAsset.InstantiateAsync(scroll.transform).Completed += (go) => {
                        productRackList.Add(go.Result);
                        SetupButtons();
                    };
                }
            }
        }
    }

    private void SetupButtons() {
        if (productRackList.Count * maxButtonInRack >= productButtonList.Count) {
            scrollRectTransform.sizeDelta = new Vector2(scroll.GetComponent<RectTransform>().rect.width, productRackList[0].GetComponent<RectTransform>().rect.height * productRackList.Count);
            scrollRectTransform.offsetMax = new Vector2(0, scrollRectTransform.offsetMax.y);
            scrollRectTransform.offsetMin = new Vector2(0, scrollRectTransform.offsetMin.y);
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
    private void Update() {
        if (controller.IsGamepad()) {
            scrollRectTransform.position -= new Vector3(0, playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed, 0);
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
