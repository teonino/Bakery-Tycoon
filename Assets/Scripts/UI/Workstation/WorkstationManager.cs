using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private AssetReference productButtonAsset;

    private GameManager gameManager;
    private List<GameObject> productButtonList;
    private List<GameObject> minigamePanelList;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigame = 0;
    public ProductSO currentProduct;

    //Create buttons
    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        workplace = FindObjectOfType<Workstation>();
        productButtonList = new List<GameObject>();
        minigamePanelList = new List<GameObject>();

        for (int i = 0; i < gameManager.GetLenghtProducts(); i++) {
            productButtonAsset.InstantiateAsync(transform).Completed += (go) => {
                go.Result.GetComponent<WorkstationButton>().workplacePanel = this;
                go.Result.GetComponent<WorkstationButton>().SetProduct(gameManager.productsList[nbButton]);
                productButtonList.Add(go.Result);
                SetupButtons();
            };
        }
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
                productButtonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20 + 120 * i, -20, 0);
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
            if (currentMinigame != currentProduct.minigames.Count)
                currentProduct.minigames[currentMinigame].InstantiateAsync(transform).Completed += (go) => {
                    minigamePanelList.Add(go.Result);
                    go.Result.name = " Panel " + currentMinigame;
                };
            else {
                currentProduct.asset.InstantiateAsync().Completed += (go) => {
                    DestroyMinigames();
                    workplace.CloseWorkplace(go.Result);
                };

                currentMinigame = 0;
                currentProduct = null;
            }
        }
    }

    public void ResetManager() {
        currentMinigame = 0;
        currentProduct = null;
        DestroyMinigames();
    }

    public void DestroyMinigames() {
        //Destroy all minigame panels
        foreach (GameObject gos in minigamePanelList) {
            Addressables.Release(gos);
        }
        minigamePanelList.Clear();
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
