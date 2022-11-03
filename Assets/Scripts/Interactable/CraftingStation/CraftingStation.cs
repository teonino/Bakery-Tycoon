using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    public CraftingStationType type;

    [SerializeField] private int dirty = 0;
    [SerializeField] private AssetReference progressBar;
    [SerializeField] private AssetReference productReady;

    private ProductSO itemInStation;
    private GameObject productReadyUI;

    public override void Effect() {
        if (playerController.itemHolded && playerController.itemHolded.tag == "Paste" && !itemInStation) {
            Product p = playerController.itemHolded.GetComponent<Product>();
            Addressables.ReleaseInstance(playerController.itemHolded);
            StartCoroutine(CookingTime(p));
        }
        else if (itemInStation && !playerController.itemHolded) {
            itemInStation.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerController.gameObject.transform.GetChild(0);
                playerController.itemHolded = go.Result;
                go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
            };
            Addressables.ReleaseInstance(productReadyUI);
            itemInStation = null;
        }
        else if (gameManager.GetDayTime() == DayTime.Evening) {
            //Check cleanness
            if (dirty > 20) {
                //Launch Animation
                playerController.DisableInput();
                progressBar.InstantiateAsync(transform).Completed += (go) => {
                    ProgressBar progressBarScript = go.Result.GetComponentInChildren<ProgressBar>();

                    go.Result.transform.localPosition = Vector3.up * 2;
                    go.Result.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    progressBarScript.SetDuration(dirty / 10);
                    progressBarScript.onDestroy.AddListener(Clean);
                };
            }
        }
    }

    private IEnumerator CookingTime(Product product) {
        progressBar.InstantiateAsync(transform).Completed += (go) => {
            go.Result.transform.localPosition = Vector3.up * 2;
            go.Result.transform.localRotation = Quaternion.Euler(0, 180, 0);
            go.Result.GetComponentInChildren<ProgressBar>().SetDuration((int)product.productSO.cookingTime);

        };
        yield return new WaitForSeconds(product.productSO.cookingTime);
        CreateProduct(product);
    }

    private void CreateProduct(Product product) {
        itemInStation = product.productSO;
        productReady.InstantiateAsync(transform).Completed += (go) => {
            go.Result.transform.localPosition = Vector3.up * 2;
            go.Result.transform.localRotation = Quaternion.Euler(0,180,0);
            productReadyUI = go.Result;
        };
    }

    public void Clean() {
        dirty = 0;
        playerController.EnableInput();
    }

    public void AddDirt() => dirty++;
}
