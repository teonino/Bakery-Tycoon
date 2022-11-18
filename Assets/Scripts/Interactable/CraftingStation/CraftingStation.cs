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

    [Header("Debug parameters")]
    [SerializeField] private bool skipCookingTime = false;

    private bool cooking = false;
    private GameObject itemInStation;
    private GameObject productReadyUI;

    public override void Effect() {
        if (playerController.itemHolded && playerController.itemHolded.tag == "Paste" && !itemInStation && playerController.itemHolded.GetComponent<Product>().GetCraftingStation() == type) {
            itemInStation = playerController.itemHolded;
            itemInStation.transform.parent = transform;
            itemInStation.transform.position = Vector3.zero;
            playerController.itemHolded = null;

            StartCoroutine(CookingTime(itemInStation.GetComponent<Product>()));
        }
        else if (itemInStation && !playerController.itemHolded && !cooking) {
            itemInStation.GetComponent<Product>().productSO.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerController.gameObject.transform.GetChild(0);
                playerController.itemHolded = go.Result;
                go.Result.GetComponent<Product>().quality = itemInStation.GetComponent<Product>().quality;
                go.Result.GetComponent<Product>().amount = itemInStation.GetComponent<Product>().amount;
                go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
                itemInStation = null;
            };
            Addressables.ReleaseInstance(productReadyUI);
        }
        else if (gameManager.dayTime == DayTime.Evening) {
            //Check cleanness
            if (dirty > 20) {
                //Launch Animation
                playerController.DisableInput();
                progressBar.InstantiateAsync(transform).Completed += (go) => {
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
        progressBar.InstantiateAsync(transform).Completed += (go) => {
            go.Result.transform.localPosition = Vector3.up * 2;
            go.Result.transform.localRotation = Quaternion.Euler(0, 180, 0);
            //go.Result.GetComponent<ProgressBar>().onDestroy = CreateProduct;

            if (skipCookingTime) 
                go.Result.GetComponentInChildren<ProgressBar>().SetDuration(0);
            else 
                go.Result.GetComponentInChildren<ProgressBar>().SetDuration((int)product.productSO.cookingTime);

        };
        cooking = true;
        if (skipCookingTime)
            yield return new WaitForSeconds(0);
        else
            yield return new WaitForSeconds(product.productSO.cookingTime);
        cooking = false;
        CreateProduct(product);
    }

    private void CreateProduct(Product product) {
        itemInStation = product.gameObject;
        
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
