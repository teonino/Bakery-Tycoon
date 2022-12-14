using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private AssetReference productButtonAsset;
    [SerializeField] private AssetReference productRackAsset;
    [SerializeField] private TextMeshProUGUI stockListText;
    [SerializeField] private GameObject stockPanel;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListIngredient ingredients; 
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private int scrollSpeed;

    private List<GameObject> productButtonList;
    private List<GameObject> productRackList;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigameCounter = 0;
    private Minigame currentMinigame;
    private int itemQuality = 0;
    private GameObject content;
    private int lenght;
    private int maxButtonInRack;

    [HideInInspector] public bool skipRequirement = false;
    [HideInInspector] public bool skipMinigame = false;
    [HideInInspector] public ProductSO currentProduct;

    private void Awake() {
        workplace = FindObjectOfType<Workstation>();
        productButtonList = new List<GameObject>();
        productRackList = new List<GameObject>();
        content = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        lenght = products.GetProductLenght();
    }

    private void OnEnable() {
        //Setup Stock
        stockListText.SetText("");
        List<StockIngredient> stocks = ingredients.GetIngredientList();
        foreach (StockIngredient stock in stocks) {
            stockListText.text += stock.ingredient.name + " : " + stock.amount + "\n";
        }

        foreach (GameObject button in productButtonList) {
            button.GetComponent<WorkstationButton>().SetRequirement(CheckRequirement(button.GetComponent<WorkstationButton>().GetProduct()));
        }

        if (productButtonList != null) {
            EnableButtons();
        }
    }

    //Create buttons
    private void Start() {
        for (int i = 0; i < lenght; i++) {
            productButtonAsset.InstantiateAsync().Completed += (go) => {
                WorkstationButton button = go.Result.GetComponent<WorkstationButton>();
                button.workplacePanel = this;
                button.SetProduct(products.GetProductList()[nbButton]);
                button.SetRequirement(CheckRequirement(products.GetProductList()[nbButton]));
                productButtonList.Add(go.Result);
                nbButton++;
                SetupRacks();
            };
        }
    }

    private void Update() {
        if (controller.IsGamepad()) {
                rectTransform.offsetMax -= new Vector2Int(0, (int)playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed);
        }
    }

    public bool CheckRequirement(ProductSO product) {
        if (skipRequirement) return true;

        bool requirementMet = true;

        //Check Crafting Station
        List<CraftingStation> craftingStations = new List<CraftingStation>(FindObjectsOfType<CraftingStation>());
        if (requirementMet) {
            requirementMet = false;
            foreach (CraftingStation craftingStation in craftingStations)
                if (craftingStation.GetCraftingStationType() == product.craftStationRequired) requirementMet = true;
        }

        //Check Ingredients
        foreach (IngredientSO ingredient in product.ingredients)
            if (ingredients.GetIngredientAmount(ingredient) <= 0) requirementMet = false;

        return requirementMet;
    }

    //Once enough button created, we position them
    private void SetupRacks() {
        if (productButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(content.GetComponent<RectTransform>().rect.width / productButtonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < productButtonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    productRackAsset.InstantiateAsync(content.transform).Completed += (go) => {
                        productRackList.Add(go.Result);
                        SetupButton();
                    };
                }
            }
        }
    }

    private void SetupButton() {
        if (productRackList.Count * maxButtonInRack >= productButtonList.Count) {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().rect.width, productRackList[0].GetComponent<RectTransform>().rect.height * productRackList.Count);
            for (int i = 0; i < lenght; i++) {
                if (i / maxButtonInRack < productRackList.Count) {
                    productButtonList[i].transform.SetParent(productRackList[i / maxButtonInRack].transform);
                    productButtonList[i].transform.localScale = Vector3.one;
                }
            }

            if (controller.IsGamepad())
                controller.SetEventSystemToStartButton(productButtonList[0]);
            else
                controller.SetEventSystemToStartButton(null);
        }
    }

    public void SetProduct(ProductSO product) {
        this.currentProduct = product;
        DisableButtons();
        LaunchMinigame();
    }

    private void LaunchMinigame() {
        if (currentProduct) {
            //Launch minigame
            if (currentMinigameCounter != currentProduct.minigames.Count && !skipMinigame)
                currentProduct.minigames[currentMinigameCounter].minigameAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = " Panel " + currentMinigameCounter;
                    currentMinigame = go.Result.GetComponent<Minigame>();
                };
            //Create product
            else {
                if (!currentProduct.hasPaste) {
                    currentProduct.asset.InstantiateAsync().Completed += (go) => {
                        if (currentMinigameCounter >= 0)
                            go.Result.GetComponent<Product>().quality = itemQuality / currentMinigameCounter;
                        workplace.CloseWorkplace(go.Result);
                        ResetManager();
                    };
                }
                else {
                    ProductSO tmpProduct = currentProduct;
                    currentProduct.pasteAsset.InstantiateAsync().Completed += (go) => {
                        if (currentMinigameCounter > 0)
                            go.Result.GetComponent<ProductHolder>().product.quality = itemQuality / currentMinigameCounter;
                        go.Result.GetComponent<ProductHolder>().product.SetProduct(tmpProduct);
                        workplace.CloseWorkplace(go.Result);
                        ResetManager();
                    };
                }

                RemoveIngredients(currentProduct);
            }
        }
    }

    private void RemoveIngredients(ProductSO product) {
        foreach (IngredientSO ingredient in product.ingredients) {
            ingredients.RemoveIngredientStock(ingredient, 1);
        }
    }

    public void ResetManager() {
        if (currentMinigame)
            currentMinigame.DisableInputs();
        currentMinigameCounter = 0;
        currentProduct = null;
        itemQuality = 0;
    }

    public void MinigameComplete(int quality) {
        itemQuality += quality;
        currentMinigame = null;
        currentMinigameCounter++;
        LaunchMinigame();
    }

    private void DisableButtons() {
        foreach (GameObject go in productButtonList)
            go.SetActive(false);
        stockPanel.SetActive(false);
    }

    private void EnableButtons() {
        foreach (GameObject go in productButtonList)
            go.SetActive(true);
        stockPanel.SetActive(true);
    }

    private void OnDestroy() {
        foreach (GameObject go in productButtonList)
            if (go)
                Addressables.ReleaseInstance(go);
    }
}
