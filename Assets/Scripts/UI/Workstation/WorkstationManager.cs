using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private AssetReference productButtonAsset;
    public bool skipRequirement = false;
    public bool skipMinigame = false;

    private GameManager gameManager;
    private List<GameObject> productButtonList;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigameCounter = 0;
    private Minigame currentMinigame;
    private int itemQuality = 0;
    [HideInInspector] public ProductSO currentProduct;

    //Create buttons
    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        workplace = FindObjectOfType<Workstation>();
        productButtonList = new List<GameObject>();

        for (int i = 0; i < gameManager.GetProductsLenght(); i++) {
            productButtonAsset.InstantiateAsync(transform).Completed += (go) => {
                WorkstationButton button = go.Result.GetComponent<WorkstationButton>();
                button.workplacePanel = this;
                button.SetProduct(gameManager.GetProductList()[nbButton]);
                button.requirementMet = CheckRequirement(gameManager.GetProductList()[nbButton]);
                productButtonList.Add(go.Result);
                SetupButtons(button.gameObject);
            };
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
                if (craftingStation.type == product.craftStationRequired) requirementMet = true;
        }

        //Check Ingredients
        foreach (IngredientSO ingredient in product.ingredients)
            if (gameManager.GetIngredientAmount(ingredient) <= 0) requirementMet = false;

        return requirementMet;
    }

    private void OnEnable() {
        if (productButtonList != null) {
            EnableButtons();
        }
    }

    //Once enough button created, we position them
    private void SetupButtons(GameObject button) {
        if (nbButton == gameManager.GetProductsLenght() - 1) {
            for (int i = 0; i < gameManager.GetProductsLenght(); i++)
                productButtonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20 + 110 * (i % 4), -20 - (110 * (i / 4)), 0);
        }

        if (nbButton == 0 && gameManager.IsGamepad())
            gameManager.SetEventSystemToStartButton(button.gameObject);
        nbButton++;
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
                        if (currentMinigameCounter == 0)
                            go.Result.GetComponent<Product>().quality = 0;
                        else
                            go.Result.GetComponent<Product>().quality = itemQuality / currentMinigameCounter;
                        workplace.CloseWorkplace(go.Result);
                        ResetManager();
                    };
                }
                else {
                    ProductSO gos = currentProduct;
                    currentProduct.pasteAsset.InstantiateAsync().Completed += (go) => {
                        if (currentMinigameCounter == 0)
                            go.Result.GetComponent<Product>().quality = 0;
                        else
                            go.Result.GetComponent<Product>().quality = itemQuality / currentMinigameCounter;
                        go.Result.GetComponent<Product>().productSO = gos;
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
            gameManager.RemoveIngredientStock(ingredient, 1);
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
    }

    private void EnableButtons() {
        foreach (GameObject go in productButtonList)
            go.SetActive(true);
    }

    private void OnDestroy() {
        foreach (GameObject go in productButtonList)
            Addressables.ReleaseInstance(go);
    }
}
