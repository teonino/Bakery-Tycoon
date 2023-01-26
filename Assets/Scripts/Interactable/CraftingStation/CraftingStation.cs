using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    [SerializeField] private AssetReference progressBarAsset;
    [SerializeField] private AssetReference readyAsset;
    [SerializeField] private Day day;
    [SerializeField] private DebugState debugState;
    [SerializeField] private CraftingStationType type;
    [SerializeField] private CreateQuest createQuest;
    [SerializeField] private CreateQuest CreateCerealQuest;

    [Header("Debug parameters")]
    [SerializeField] private bool skipCookingTime = false;

    private bool cooking = false;
    private Product itemInStation;
    private GameObject readyText;

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
            CookingTime(itemInStation);
        }
        else if (itemInStation != null && !playerControllerSO.GetPlayerController().GetItemHold() && !cooking) {
            itemInStation.productSO.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;
                playerControllerSO.GetPlayerController().SetItemHold(go.Result);
                ProductHolder productItem = go.Result.GetComponent<ProductHolder>();

                productItem.product.amount = itemInStation.amount;
                go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = Vector3.zero;

                createQuest?.CheckProduct(productItem.product.productSO);
                CreateCerealQuest?.CheckProduct(productItem.product.productSO);

                itemInStation = null;
                Addressables.ReleaseInstance(readyText);
            };
        }
        //else if (day.GetDayTime() == DayTime.Evening) {
        //    //Check cleanness
        //    if (dirty > 20) {
        //        //Launch Animation
        //        playerControllerSO.GetPlayerController().DisableInput();
        //        progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
        //            ProgressBar progressBarScript = go.Result.GetComponentInChildren<ProgressBar>();

        //            go.Result.transform.localPosition = Vector3.up * 2;
        //            go.Result.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, 0);
        //            progressBarScript.SetDuration(dirty / 10);
        //            progressBarScript.CanBurn(false);
        //            progressBarScript.onDestroy = Clean;
        //        };
        //    }
        //}
    }

    private void CookingTime(Product product) {
        progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
            GameObject progressBar = go.Result;
            go.Result.transform.localPosition = Vector3.up * 2;
            go.Result.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, 0);

            ProgressBar progressBarScript = progressBar.GetComponentInChildren<ProgressBar>();

            if (skipCookingTime)
                progressBarScript.SetDuration(0);
            else
                progressBarScript.SetDuration((int)product.productSO.cookingTime);

            progressBarScript.onDestroy = FinishCooking;
        };

        cooking = true;

        readyAsset.InstantiateAsync().Completed += (go) => {
            readyText = go.Result;
            readyText.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            readyText.SetActive(false);
        };
        //if (skipCookingTime)
        //    yield return new WaitForSeconds(0);
        //else
        //    yield return new WaitForSeconds(product.productSO.cookingTime);
        //cooking = false;
    }

    private void FinishCooking() {
        cooking = false;
        readyText.SetActive(true);
    }

    //public void Clean() {
    //    dirty = 0;
    //    playerControllerSO.GetPlayerController().EnableInput();
    //}

    //public void AddDirt() => dirty++;
}
