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
        item.transform.localPosition = Vector3.up * 1.25f;
        base.TakeItem(product, displayGO);
        Leave();
    }

    protected IEnumerator CustomerWaiting(float time) {
        yield return new WaitForSeconds(time);
        Leave();
    }

    private new void Leave() {
        if (!item)
            reputation.RemoveReputation(3);
        base.Leave();
        shelf.RemoveCustomerInQueue(this);
    }

    new void FixedUpdate() {
        //Go to the Queue
        if (agent.remainingDistance < 1 && state == AIState.moving) {
            state = AIState.waiting;
        }

        //Buy item and leave
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(shelf.transform.position.x, shelf.transform.position.z)) < 2 && shelf.GetItem() && state == AIState.waiting && shelf.IsFirstInQueue(this)) {
            ProductHolder objectOnShelf = shelf.GetItem().GetComponent<ProductHolder>();
            if (objectOnShelf.product.GetName() == requestedProduct.name && shelf.GetItem().tag != "Paste") {
                //Take item
                if (!item) {
                    if (objectOnShelf.product.amount > 1) {
                        objectOnShelf.product.productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            TakeItem(objectOnShelf, shelf.gameObject);
                        };
                        objectOnShelf.product.amount--;
                    }
                    else {
                        item = shelf.GetItem();
                        item.transform.SetParent(transform);
                        TakeItem(objectOnShelf, shelf.gameObject);
                        shelf.RemoveItem();
                    }
                }
            }
        }
        base.FixedUpdate();
    }

    public override void Effect() {
        print("Effect");
        Leave();
    }
}
