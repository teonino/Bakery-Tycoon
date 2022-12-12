using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    [SerializeField] private AssetReference progressBarAsset;
    [SerializeField] private Day day;
    [SerializeField] private DebugState debugState;
    [SerializeField] private CraftingStationType type;
    [SerializeField] private int dirty = 0;

    [Header("Debug parameters")]
    [SerializeField] private bool skipCookingTime = false;

    private bool cooking = false;
    private Product itemInStation;
    private GameObject progressBar;

    private void Start() {
        if (!debugState.GetDebug())
            skipCookingTime = false;
    }

    public CraftingStationType GetCraftingStationType() => type;

    public override void Effect() {
        if (playerControllerSO.GetPlayerController().GetItemHold() && playerControllerSO.GetPlayerController().GetItemHold().tag == "Paste" && itemInStation == null && playerControllerSO.GetPlayerController().GetItemHold().GetComponent<ProductHolder>().product.GetCraftingStation() == type) {
            itemInStation = new Product(playerControllerSO.GetPlayerController().GetItemHold().GetComponent<ProductHolder>().product);
            Addressables.ReleaseInstance(playerControllerSO.GetPlayerController().GetItemHold());
            playerControllerSO.GetPlayerController().SetItemHold(null);
            StartCoroutine(CookingTime(itemInStation));
        }
        else if (itemInStation != null && !playerControllerSO.GetPlayerController().GetItemHold() && !cooking) {
            itemInStation.productSO.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;
                playerControllerSO.GetPlayerController().SetItemHold(go.Result);
                ProductHolder productItem = go.Result.GetComponent<ProductHolder>();

                if (progressBar.GetComponent<ProgressBar>().burned)
                    productItem.product.quality = 0;
                else
                    productItem.product.quality = itemInStation.quality;
                productItem.product.amount = itemInStation.amount; go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = Vector3.zero;
                itemInStation = null;
                Addressables.ReleaseInstance(progressBar);
            };            
        }
        else if (day.GetDayTime() == DayTime.Evening) {
            //Check cleanness
            if (dirty > 20) {
                //Launch Animation
                playerControllerSO.GetPlayerController().DisableInput();
                progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
                    ProgressBar progressBarScript = go.Result.GetComponentInChildren<ProgressBar>();

                    go.Result.transform.localPosition = Vector3.up * 2;
                    go.Result.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, 0);
                    progressBarScript.SetDuration(dirty / 10);
                    progressBarScript.CanBurn(false);
                    progressBarScript.onDestroy = Clean;
                };
            }
        }
    }

    private IEnumerator CookingTime(Product product) {
        progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
            progressBar = go.Result;
            go.Result.transform.localPosition = Vector3.up * 2;
            go.Result.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, 0);

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
        playerControllerSO.GetPlayerController().EnableInput();
    }

    public void AddDirt() => dirty++;
}
