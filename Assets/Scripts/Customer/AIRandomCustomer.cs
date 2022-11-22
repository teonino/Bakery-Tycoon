using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIRandomCustomer : AICustomer {
    protected MainShelf shelf;
    [HideInInspector] public bool inQueue = false;

    private new void Awake() {
        base.Awake();
        shelf = FindObjectOfType<MainShelf>();
        //Check Queue positions
        shelf.GetAvailableQueuePosition(this);
    }

    public new void InitCustomer() {
        if (inQueue)
            base.InitCustomer();
        else
            DestroyCustomer();
        state = AIState.moving;
    }

    private new void TakeItem(ProductHolder product, GameObject displayGO) {
        base.TakeItem(product, displayGO);
        Leave();
    }

    protected IEnumerator CustomerWaiting(float time) {
        yield return new WaitForSeconds(time);
        Leave();
    }

    private new void Leave() {
        base.Leave();
        shelf.RemoveCustomerInQueue(this);
    }

    new void FixedUpdate() {
        //Go to the Queue
        if (agent.remainingDistance < 1 && state == AIState.moving) {
            state = AIState.waiting;
            waitingCoroutine = StartCoroutine(CustomerWaiting(waitingTime, Leave));
        }

        //Buy item and leave
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(shelf.transform.position.x, shelf.transform.position.z)) < 2 && shelf.item && state == AIState.waiting && shelf.IsFirstInQueue(this)) {
            if (shelf.GetItem().GetComponent<ProductHolder>().product.GetName() == requestedProduct.name && shelf.GetItem().GetComponent<ProductHolder>().tag != "Paste") {
                //Stop waiting
                StopCoroutine(waitingCoroutine);
                //Take item
                if (!item) {
                    if (shelf.GetItem().GetComponent<ProductHolder>().product.amount > 1) {
                        shelf.GetItem().GetComponent<ProductHolder>().product.productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            TakeItem(shelf.item.GetComponent<ProductHolder>(), shelf.gameObject);
                        };
                        shelf.GetItem().GetComponent<ProductHolder>().product.amount--;
                    }
                    else {
                        item = shelf.GetItem();
                        item.transform.SetParent(transform);
                        TakeItem(shelf.item.GetComponent<ProductHolder>(), shelf.gameObject);
                        shelf.RemoveItem();
                    }
                }
            }
        }

        base.FixedUpdate();
    }
}
