using System;
using System.Collections;
using UnityEngine;

public class AIRandomCustomer : AICustomer
{
    protected MainShelf shelf;
    [HideInInspector] public bool inQueue = false;

    private QueueBakery interacting;
    private bool hasInteract = false;
    public QueueBakery GetInteracting() => interacting;
    public void SetInteracting(QueueBakery interacting) => this.interacting = interacting;

    private new void Awake()
    {
        base.Awake();
        shelf = FindObjectOfType<MainShelf>();
        //Check Queue positions
        shelf.GetAvailableQueuePosition(this);
    }

    public override void InitCustomer()
    {
        base.InitCustomer();

        day.DayTimeChange += LeaveOnEvening;
        state = AIState.moving;
        //animator.SetTrigger("Walk");
    }

    private new void TakeItem(ProductHolder product, GameObject displayGO)
    {
        item.transform.localPosition = Vector3.up * 1.25f;
        base.TakeItem(product, displayGO);
        Leave();
    }

    protected IEnumerator CustomerWaiting(float time)
    {
        yield return new WaitForSeconds(time);
        Leave();
    }

    private void LeaveOnEvening()
    {
        if (day.GetDayTime() == DayTime.Evening)
        {
            Leave();
            day.DayTimeChange -= LeaveOnEvening;
        }
    }

    protected override void Leave()
    {
        base.Leave();
        shelf.RemoveCustomerInQueue(this);
    }

    new void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) < 0.5 && agent.speed != 0) {
            transform.rotation.SetLookRotation(agent.destination);
            agent.speed = 0;
        }


        //Go to the Queue
        if (Vector3.Distance(transform.position, agent.destination) < 0.5 && state == AIState.moving)
        {
            state = AIState.waiting;
            coroutine = StartCoroutine(CustomerWaiting(waitingTime, Leave));

            if (!interacting) {
                //animator.SetTrigger("Idle");
            }
        }

        if (interacting && Vector3.Distance(transform.position, agent.destination) < 0.2 && !hasInteract) {
            //interacting.Interact(animator); // trigger animation according to item
            hasInteract = true;
        }

        //Buy item and leave
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(shelf.transform.position.x, shelf.transform.position.z)) < 2 && shelf.GetItem() && state == AIState.waiting && shelf.IsFirstInQueue(this))
        {
            ProductHolder objectOnShelf = shelf.GetItem().GetComponent<ProductHolder>();
            if (objectOnShelf.product.GetName() == requestedProduct.name && shelf.GetItem().tag != "Paste")
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                //Take item
                if (!item)
                {
                    if (objectOnShelf.product.amount > 1)
                    {
                        objectOnShelf.product.productSO.asset.InstantiateAsync(transform).Completed += (go) =>
                        {
                            item = go.Result;
                            TakeItem(objectOnShelf, shelf.gameObject);
                        };
                        objectOnShelf.product.amount--;
                    }
                    else
                    {
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

    public Animator GetAnimator() => animator;

    public override void Effect()
    {
        reputation.RemoveReputation(5);
        Leave();
    }
}
