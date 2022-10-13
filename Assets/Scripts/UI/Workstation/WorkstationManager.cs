using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AssetReference productButton;

    private List<GameObject> productButtonList;
    private List<GameObject> minigamesPanels;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigame = 0;
    public ProductSO product;

    //Create buttons
    private void Start() {
        workplace = FindObjectOfType<Workstation>();
        productButtonList = new List<GameObject>();
        minigamesPanels = new List<GameObject>();

        for (int i = 0; i < gameManager.GetLenghtProducts(); i++) {
            productButton.InstantiateAsync(transform).Completed += (go) => {
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
        this.product = product;
        DisableButtons();
        LaunchMinigame();
    }

    private void LaunchMinigame() {
        if (currentMinigame != product.minigames.Count)
            product.minigames[currentMinigame].InstantiateAsync(transform).Completed += (go) => {
                minigamesPanels.Add(go.Result);
                go.Result.name = " Panel " + currentMinigame;
            };
        else {
            product.asset.InstantiateAsync().Completed += (go) => {
                DestroyMinigames();
                workplace.CloseWorkplace(go.Result);
            };

            currentMinigame = 0;
            product = null;
        }
    }

    public void ResetManager() {
        currentMinigame = 0;
        product = null;
        DestroyMinigames();
    }

    public void DestroyMinigames() {
        //Destroy all minigame panels
        foreach (GameObject gos in minigamesPanels) {
            Addressables.Release(gos);
        }
        minigamesPanels.Clear();
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
}
