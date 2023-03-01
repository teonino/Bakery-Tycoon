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

    [SerializeField] private AudioSource BurningSound;
    [SerializeField] private AudioSource TingSound;
    [SerializeField] private SFXPlayer sfxPlayer;

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
            sfxPlayer.InteractSound();
        }
        else if (itemInStation != null && !playerControllerSO.GetPlayerController().GetItemHold() && !cooking) {
            itemInStation.productSO.asset.InstantiateAsync().Completed += (go) => {
                Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;
                playerControllerSO.GetPlayerController().SetItemHold(go.Result);
                ProductHolder productItem = go.Result.GetComponent<ProductHolder>();

                productItem.product.SetAmount(itemInStation.GetAmount());
                go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = Vector3.zero;

                createQuest?.CheckProduct(productItem.product.productSO);
                CreateCerealQuest?.CheckProduct(productItem.product.productSO);

                itemInStation = null;
                Addressables.ReleaseInstance(readyText);
            };
        }
        else if (playerControllerSO.GetPlayerController().GetItemHold() && playerControllerSO.GetPlayerController().GetItemHold().tag == "Paste" && playerControllerSO.GetPlayerController().GetItemHold().GetComponent<ProductHolder>().product.GetCraftingStation() == type && itemInStation != null && !cooking) {
            itemInStation.productSO.asset.InstantiateAsync().Completed += (go) => {
                //New Product being cooked
                Product oldItemInStation = itemInStation;
                itemInStation = new Product(playerControllerSO.GetPlayerController().GetItemHold().GetComponent<ProductHolder>().product);
                Addressables.ReleaseInstance(playerControllerSO.GetPlayerController().GetItemHold()); 
                CookingTime(itemInStation);
                sfxPlayer.InteractSound();

                //Old product being held by player
                Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;
                playerControllerSO.GetPlayerController().SetItemHold(go.Result);
                ProductHolder productItem = go.Result.GetComponent<ProductHolder>();
                productItem.product.SetAmount(oldItemInStation.GetAmount());
                go.Result.transform.SetParent(arm);
                go.Result.transform.localPosition = Vector3.zero; 
                Addressables.ReleaseInstance(readyText);

                createQuest?.CheckProduct(productItem.product.productSO);
                CreateCerealQuest?.CheckProduct(productItem.product.productSO);
            };
        }
    }

    private void CookingTime(Product product) {
        progressBarAsset.InstantiateAsync(transform).Completed += (go) => {
            TingSound.Stop();
            BurningSound.Play();
            GameObject progressBar = go.Result;
            go.Result.transform.localPosition = Vector3.up * 2.3f;
            go.Result.GetComponent<RectTransform>().rotation = Quaternion.Euler(90, 0, 0);

            ProgressBar progressBarScript = progressBar.GetComponentInChildren<ProgressBar>();

            if (skipCookingTime)
                progressBarScript.SetDuration(0);
            else
                progressBarScript.SetDuration((int)product.productSO.cookingTime);

            progressBarScript.onDestroy += FinishCooking;
        };

        cooking = true;

        readyAsset.InstantiateAsync().Completed += (go) => {
            readyText = go.Result;
            readyText.transform.position = new Vector3(transform.position.x, transform.position.y + 2.3f, transform.position.z);
            readyText.SetActive(false);
        };
        //if (skipCookingTime)
        //    yield return new WaitForSeconds(0);
        //else
        //    yield return new WaitForSeconds(product.productSO.cookingTime);
        //cooking = false;
    }

    private void FinishCooking() {
        BurningSound.Stop();
        TingSound.Play();
        cooking = false;
        readyText.SetActive(true);
    }

    //public void Clean() {
    //    dirty = 0;
    //    playerControllerSO.GetPlayerController().EnableInput();
    //}

    //public void AddDirt() => dirty++;
}
