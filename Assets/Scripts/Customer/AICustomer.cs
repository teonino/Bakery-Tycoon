using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.UI;

public class AICustomer : Interactable {
    [Header("AI Customer References")]
    [SerializeField] protected AssetReference assetProductCanvas;
    [SerializeField] protected AssetReference assetPaymentCanvas;
    [SerializeField] protected NavMeshAgent agent;
    [Tooltip("Time in which if you give the requested product, earn a bonus of money & reputation")]
    [SerializeField] protected int bonusTime;
    [SerializeField] protected Money money;
    [SerializeField] protected Reputation reputation;
    [SerializeField] protected Statistics stats;
    [SerializeField] protected Day day;

    [Header("AI Customer Variables")]
    [SerializeField] protected float waitingTime = 5f;
    [SerializeField] protected int saleReputation;

    [HideInInspector] public ProductSO requestedProduct;

    public AIState state = AIState.idle;

    protected bool bonus = true;
    protected GameObject productCanvas;
    protected GameObject item;
    protected SpawnCustomer spawner;
    protected Vector3 spawnPosition;
    protected bool tutorial;

    protected void Awake() {
        spawner = FindObjectOfType<SpawnCustomer>();

        day = FindObjectOfType<DayTimeUI>().GetDay();
        reputation = FindObjectOfType<ReputationUI>().GetReputation();
        money = FindObjectOfType<MoneyUI>().GetMoney();
    }

    public void InitCustomer() {

        assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {
            productCanvas = go.Result;
            productCanvas.transform.SetParent(transform);
            productCanvas.transform.position = transform.position + Vector3.up;
            if (requestedProduct) 
                productCanvas.GetComponentInChildren<RawImage>().texture = requestedProduct.image;
            else
                Debug.LogError("RequestedProductNull");

            StartCoroutine(LaunchBonusTime());
        };
        spawnPosition = transform.position;
    }

    private IEnumerator LaunchBonusTime() {
        yield return new WaitForSeconds(bonusTime);
        bonus = false;

    }

    protected void FixedUpdate() {
        //Exit the bakery
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(spawnPosition.x, spawnPosition.z)) < 2 && state == AIState.leaving)
            DestroyCustomer();
    }

    public void TakeItem(ProductHolder product, GameObject displayGo) {
        item.GetComponent<ProductHolder>().product.quality = product.product.quality;

        stats.AddProductSold(item.GetComponent<ProductHolder>().product.productSO);
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

    public void SetWaitingTime(int time) {
        tutorial = true;
        waitingTime = time;
    }

    protected IEnumerator CustomerWaiting(float time, Action leavingFunction) {
        yield return new WaitForSeconds(time);
        leavingFunction.Invoke();
    }

    //Remove product panel + exit bakery
    protected virtual void Leave() {
        state = AIState.leaving;
        agent.SetDestination(spawnPosition);
    }

    //Display the payement
    public void DisplayPayment(GameObject displayGO) {
        int basePrice = item.GetComponent<ProductHolder>().product.productSO.price;
        //int totalPrice = basePrice + basePrice * item.GetComponent<ProductHolder>().product.quality / 100;

        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = displayGO.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<PaymentCanvasManager>().Init(basePrice, 0);
            requestedProduct = null;
        };

        money.AddMoney(basePrice);
        reputation.AddReputation(saleReputation);
    }

    public void SetDestination(Vector3 position) => agent.SetDestination(position);

    public override void Effect() { }
}


