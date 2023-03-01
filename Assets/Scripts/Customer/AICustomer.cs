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
    [SerializeField] protected Statistics stats;
    [SerializeField] protected CustomerBonusSO customerBonus;
    [SerializeField] protected Animator animator;

    [Header("AI Customer Variables")]
    [SerializeField] protected CustomerWaitingTime waitingTime;
    [SerializeField] protected int saleReputation;

    [HideInInspector] public ProductSO requestedProduct;

    public AIState state = AIState.idle;
    protected CustomerInteractable interactable;
    protected Money money;
    protected Reputation reputation;
    protected Day day;
    protected bool bonus = true;
    protected GameObject productCanvas;
    protected GameObject item;
    protected SpawnCustomer spawner;
    protected Vector3 spawnPosition;
    protected bool tutorial;
    protected Coroutine coroutine;

    protected void Awake() {
        day = FindObjectOfType<DayTimeUI>().GetDay();
        money = FindObjectOfType<MoneyUI>().GetMoney();
        reputation = FindObjectOfType<ReputationUI>().GetReputation();
    }

    public void SetSpawner(SpawnCustomer spawner) => this.spawner = spawner;

    public virtual void InitCustomer() {
        assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {
            productCanvas = go.Result;
            productCanvas.transform.SetParent(transform);
            productCanvas.transform.position = transform.position + Vector3.up * 2;
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
        if (state == AIState.leaving && (transform.position - spawnPosition).magnitude < 4)
            DestroyCustomer();
    }


    public virtual bool isRegular() { return false; }

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
    }

    protected IEnumerator CustomerWaiting(float time, Action leavingFunction) {
        if (state != AIState.eating)
            spawner.LaunchCommandRecap(this);
        yield return new WaitForSeconds(time);
        leavingFunction.Invoke();
    }

    //Remove product panel + exit bakery
    protected virtual void Leave() {
        state = AIState.leaving;
        if (agent) {
            agent.speed = 2;
            agent.SetDestination(spawnPosition);
        }

        spawner.RemoveCommandRecap(this);
    }

    //Display the payement
    public void DisplayPayment(GameObject displayGO) {
        int basePrice = item.GetComponent<ProductHolder>().product.productSO.price;
        int totalPrice = (int)Mathf.Ceil(basePrice + basePrice * customerBonus.GetMoneyMultiplier());

        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = displayGO.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<PaymentCanvasManager>().Init(totalPrice, 0);
            requestedProduct = null;
        };

        money.AddMoney(totalPrice);
        reputation.AddReputation(saleReputation);
    }

    public void SetDestination(Vector3 position) {
        agent.SetDestination(position);
        agent.speed = 2;
    }
    public void HideCanvas() {
        productCanvas.gameObject.SetActive(false);
    }

    public void DispalyCanvas() {
        productCanvas.gameObject.SetActive(true);
    }
    public NavMeshAgent GetAgent() => agent;

    public override void Effect() { }
}


