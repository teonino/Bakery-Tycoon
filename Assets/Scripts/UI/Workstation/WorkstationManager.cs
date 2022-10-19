using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private AssetReference productButtonAsset;

    private GameManager gameManager;
    private List<GameObject> productButtonList;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigame = 0;
    public ProductSO currentProduct;

    //Create buttons
    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        workplace = FindObjectOfType<Workstation>();
        productButtonList = new List<GameObject>();

        for (int i = 0; i < gameManager.GetLenghtProducts(); i++) {
            productButtonAsset.InstantiateAsync(transform).Completed += (go) => {
                WorkstationButton button = go.Result.GetComponent<WorkstationButton>();
                button.workplacePanel = this;
                button.SetProduct(gameManager.productsList[nbButton]);
                button.requirementMet = CheckRequirement(gameManager.productsList[nbButton]);
                productButtonList.Add(go.Result);
                SetupButtons();
            };
        }
    }

    public bool CheckRequirement(ProductSO product) {
        bool requirementMet = true;
        List<CraftingStation> craftingStations = new List<CraftingStation>(FindObjectsOfType<CraftingStation>());

        //Check Crafting Station
        if (product.hoven && requirementMet) { // If hoven is a requirement and requirementMet is still true
            requirementMet = false;
            foreach (CraftingStation craftingStation in craftingStations) {
                if (craftingStation.type == CraftingStationType.Hoven) {
                    requirementMet = true;
                }
            }
        }

        //Check Ingredients
        foreach (IngredientSO ingredient in product.ingredients) {
            if (gameManager.GetIngredientAmount(ingredient) == 0) {
                requirementMet = false;
            }
        }

        return requirementMet;
    }

    private void OnEnable() {
        if (productButtonList != null) {
            EnableButtons();
        }
    }

    //Once enough button created, we position them
    private void SetupButtons() {
        if (nbButton == gameManager.GetLenghtProducts() - 1) {
            for (int i = 0; i < gameManager.GetLenghtProducts(); i++)
                productButtonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20 + 110 * (i % 4), -20 - (110 * (i / 4)), 0);
        }
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
            if (currentMinigame != currentProduct.minigames.Count)
                currentProduct.minigames[currentMinigame].minigameAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = " Panel " + currentMinigame;
                };
            //Create product
            else {
                if (currentProduct.pasteAsset == null) {
                    currentProduct.asset.InstantiateAsync().Completed += (go) => {
                        workplace.CloseWorkplace(go.Result);
                    };
                }
                else {
                    ProductSO gos = currentProduct;
                    currentProduct.pasteAsset.InstantiateAsync().Completed += (go) => {
                        go.Result.GetComponent<Product>().product = gos;
                        workplace.CloseWorkplace(go.Result);
                    };
                }

                RemoveIngredients(currentProduct);
                currentMinigame = 0;
                currentProduct = null;
            }
        }
    }

    private void RemoveIngredients(ProductSO product) {
        foreach (IngredientSO ingredient in product.ingredients) {
            gameManager.RemoveIngredientStock(ingredient, 1);
        }
    }

    public void ResetManager() {
        currentMinigame = 0;
        currentProduct = null;
    }

    public void MinigameComplete() {
        currentMinigame++;
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
