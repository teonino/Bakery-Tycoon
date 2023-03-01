using System;
using System.Collections;
using UnityEngine;

public class AIRandomCustomer : AICustomer {
    protected MainShelf shelf;
    [HideInInspector] public bool inQueue = false;

    private QueueBakery interacting;
    private bool hasInteract = false;
    private bool hasTakenItem = false;
    public QueueBakery GetInteracting() => interacting;
    public void SetInteracting(QueueBakery interacting) => this.interacting = interacting;

    private new void Awake() {
        base.Awake();
        shelf = FindObjectOfType<MainShelf>();
        //Check Queue positions
        shelf.GetAvailableQueuePosition(this);
    }

    public override void InitCustomer() {
        base.InitCustomer();

        day.DayTimeChange += LeaveOnEvening;
        state = AIState.moving;
        //animator.SetTrigger("Walk");
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

    private void LeaveOnEvening() {
        if (day.GetDayTime() == DayTime.Evening) {
            Leave();
            day.DayTimeChange -= LeaveOnEvening;
        }
    }

    protected override void Leave() {
        base.Leave();
        shelf.RemoveCustomerInQueue(this);
    }

    new void FixedUpdate() {
        if (agent.speed != 0 && Vector3.Distance(transform.position, agent.destination) < 0.5) {
            transform.rotation.SetLookRotation(agent.destination);
            agent.speed = 0;
        }


        //Go to the Queue
        if (state == AIState.moving && Vector3.Distance(transform.position, agent.destination) < 0.5) {
            state = AIState.waiting;
            coroutine = StartCoroutine(CustomerWaiting(waitingTime.GetWaitingTime(), Leave));

            if (!interacting) {
                //animator.SetTrigger("Idle");
            }
        }

        //If too far away from destination, retrigger movements
        if (state != AIState.moving && Vector3.Distance(transform.position, agent.destination) > 2) {
            SetDestination(agent.destination);
        }

        if (interacting && !hasInteract && Vector3.Distance(transform.position, agent.destination) < 0.2 ) {
            //interacting.Interact(animator); // trigger animation according to item
            hasInteract = true;
        }

        //Buy item and leave
        if (shelf.GetItem() && state == AIState.waiting && shelf.IsFirstInQueue(this) && (transform.position - shelf.transform.position).magnitude < 4) {
            ProductHolder objectOnShelf = shelf.GetItem().GetComponent<ProductHolder>();
            if (objectOnShelf.product.GetName() == requestedProduct.name && shelf.GetItem().tag != "Paste") {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                //Take item
                if (!item && !hasTakenItem) {
                    hasTakenItem = true;
                    if (objectOnShelf.product.GetAmount() > 1) {
                        objectOnShelf.product.productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            item.GetComponent<ProductHolder>().DisplayOneProduct();
                            TakeItem(objectOnShelf, shelf.gameObject);
                        };
                        objectOnShelf.RemoveAmount();
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


    public override bool isRegular() {
        return false;
    }

    public Animator GetAnimator() => animator;

    public override void Effect() {
        reputation.RemoveReputation(5);
        Leave();
    }
}
