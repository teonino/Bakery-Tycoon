using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AICustomer : Interactable {
    [SerializeField] protected AssetReference assetProductCanvas;
    [SerializeField] protected AssetReference assetPaymentCanvas;
    [SerializeField] protected float waitingTime = 5f; 
    [SerializeField] protected NavMeshAgent agent;

    [HideInInspector] public ProductSO requestedProduct;

    public AIState state = AIState.idle;
    protected GameObject productCanvas; 
    protected GameObject item;
    protected SpawnCustomer spawner;
    protected Vector3 spawnPosition; 
    protected Coroutine waitingCoroutine;

    protected new void Awake() {
        base.Awake();
        gameManager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<SpawnCustomer>();
    }

    public void InitCustomer() {
        assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {
            productCanvas = go.Result;
            productCanvas.transform.SetParent(transform);
            productCanvas.transform.position = transform.position + Vector3.up * 2;
            if (requestedProduct)
                productCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText(requestedProduct.name);
            else
                print("requestedProductNull");
        };
        spawnPosition = transform.position;
    }

    protected void FixedUpdate() {
        //Exit the bakery
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(spawnPosition.x, spawnPosition.z)) < 2 && state == AIState.leaving)
            DestroyCustomer();
    }

    public void TakeItem(ProductHolder product, GameObject displayGo) {
        item.GetComponent<ProductHolder>().product.quality = product.product.quality;

        gameManager.AddProductSold(item.GetComponent<ProductHolder>().product.productSO);
        if (productCanvas)
            Addressables.ReleaseInstance(productCanvas);
        DisplayPayment(displayGo);
    }

    protected void DestroyCustomer() {
        if (item)
            Addressables.ReleaseInstance(item);
        spawner.RemoveCustomer();
        Addressables.ReleaseInstance(gameObject);
    }

    protected IEnumerator CustomerWaiting(float time, Action leavingFunction) {
        yield return new WaitForSeconds(time);
        leavingFunction.Invoke();
    }

    //Remove product panel + exit bakery
    protected void Leave() {
        state = AIState.leaving;
        agent.SetDestination(spawnPosition);
    }

    //Display the payement
    public void DisplayPayment(GameObject displayGO) {
        int totalPrice = gameManager.GetProductPrice(item.GetComponent<ProductHolder>().product.productSO) + gameManager.GetProductPrice(item.GetComponent<ProductHolder>().product.productSO) * item.GetComponent<ProductHolder>().product.quality / 100;

        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = displayGO.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "+" + totalPrice + "€";
            requestedProduct = null;
        };

        gameManager.AddMoney(totalPrice);
        gameManager.AddReputation(1);
    }

    public void SetDestination(Vector3 position) => agent.SetDestination(position);

    public override void Effect() {

    }
}


