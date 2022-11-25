using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    public CraftingStationType type;

    [SerializeField] private int dirty = 0;
    [SerializeField] private AssetReference progressBarAsset;

    [Header("Debug parameters")]
    [SerializeField] private bool skipCookingTime = false;

    private bool cooking = false;
    private Product itemInStation;
    private GameObject progressBar;

    public override void Effect() {
        if (playerController.itemHolded && playerController.itemHolded.tag == "Paste" && itemInStation == null && playerController.itemHolded.GetComponent<ProductHolder>().product.GetCraftingStation() == type) {
            itemInStation = new Product(playerController.itemHolded.GetComponent<ProductHolder>().product);
            Addressables.ReleaseInstance(playerController.itemHolded);
            playerController.itemHolded = null;

            StartCoroutine(CookingTime(itemInStation));
        }
        else if (itemInStation != null && !playerController.itemHolded && !cooking) {
            itemInStation.productSO.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerController.gameObject.transform.GetChild(0);
                playerController.itemHolded = go.Result;
                if (progressBar.GetComponent<ProgressBar>().burned)
                    go.Result.GetComponent<ProductHolder>().product.quality = 0;
                else
                    go.Result.GetComponent<ProductHolder>().product.quality = itemInStation.quality;
                go.Result.GetComponent<ProductHolder>().product.amount = itemInStation.amount; go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
                itemInStation = null;
                Addressables.ReleaseInstance(progressBar);
            };            
        }
        else if (gameManager.dayTime == DayTime.Evening) {
            //Check cleanness
            if (dirty > 20) {
                //Launch Animation
                playerController.DisableInput();
                progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
                    ProgressBar progressBarScript = go.Result.GetComponentInChildren<ProgressBar>();

                    go.Result.transform.localPosition = Vector3.up * 2;
                    go.Result.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    progressBarScript.SetDuration(dirty / 10);
                    progressBarScript.onDestroy = Clean;
                };
            }
        }
    }

    private IEnumerator CookingTime(Product product) {
        progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
            progressBar = go.Result;
            progressBar.transform.localPosition = Vector3.up * 2;
            progressBar.transform.localRotation = Quaternion.Euler(0, 180, 0);

            if (skipCookingTime)
                progressBar.GetComponentInChildren<ProgressBar>().SetDuration(0);
            else
                progressBar.GetComponentInChildren<ProgressBar>().SetDuration((int)product.productSO.cookingTime);

        };
        cooking = true;
        if (skipCookingTime)
            yield return new WaitForSeconds(0);
        else
            yield return new WaitForSeconds(product.productSO.cookingTime);
        cooking = false;
    }

    public void Clean() {
        dirty = 0;
        playerController.EnableInput();
    }

    public void AddDirt() => dirty++;
}
