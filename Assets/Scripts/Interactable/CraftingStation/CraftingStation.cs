using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    public CraftingStationType type;

    [SerializeField] private int dirty = 0;
    [SerializeField] private AssetReference progressBar;

    private ProductSO item;

    public void AddDirt() {
        dirty++;
    }

    public override void Effect() {
        if (playerController.itemHolded && playerController.itemHolded.tag == "Paste") {
            Product p = playerController.itemHolded.GetComponent<Product>();
            Addressables.ReleaseInstance(playerController.itemHolded);
            StartCoroutine(CookingTime(p));
        }
        else if (item && !playerController.itemHolded) {
            item.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerController.gameObject.transform.GetChild(0);
                playerController.itemHolded = go.Result;
                go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
            };
            item = null;
        }
        else {
            //Check cleanness
            if (dirty > 20) {
                //Launch Animation
                playerController.DisableInput();
                progressBar.InstantiateAsync(transform).Completed += (go) => {
                    ProgressBar progressBarScript = go.Result.GetComponentInChildren<ProgressBar>();

                    go.Result.transform.localPosition = Vector3.up;
                    progressBarScript.SetDuration(dirty / 10);
                    progressBarScript.onDestroy.AddListener(Clean);
                };
            }
        }
    }

    private IEnumerator CookingTime(Product product) {
        progressBar.InstantiateAsync(transform).Completed += (go) => {
            go.Result.transform.localPosition = Vector3.up;
            go.Result.GetComponentInChildren<ProgressBar>().SetDuration((int)product.product.cookingTime);

        };
        yield return new WaitForSeconds(product.product.cookingTime);
        CreateProduct(product);
    }

    private void CreateProduct(Product product) {
        item = product.product;
    }

    public void Clean() {
        dirty = 0;
        playerController.EnableInput();
    }
}
