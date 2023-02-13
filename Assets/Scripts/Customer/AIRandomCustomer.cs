using System;
using System.Collections;
using UnityEngine;

public class AIRandomCustomer : AICustomer
{
    protected MainShelf shelf;
    [HideInInspector] public bool inQueue = false;
    [SerializeField] private Animator animator;

    private new void Awake()
    {
        base.Awake();
        shelf = FindObjectOfType<MainShelf>();
        //Check Queue positions
        shelf.GetAvailableQueuePosition(this);

        if (!inQueue)
            FindInteraction();
    }

    private void FindInteraction()
    {
        foreach(CustomerInteractable interactable in listCustomerInteractable)
        {
            if (!interactable.HasCustomer())
            {
                this.interactable = interactable;
                interactable.SetCustomer(this);
                agent.SetDestination(interactable.GetPosition());
            }
        }
    }

    public override void InitCustomer()
    {
        base.InitCustomer();

        day.DayTimeChange += LeaveOnEvening;
        state = AIState.moving;
        animator.SetTrigger("Walk");
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
        //Go to the Queue
        if ((Vector3.Distance(transform.position, agent.destination) < 1 && state == AIState.moving))
        {
            state = AIState.waiting;
            animator.SetTrigger("Idle");
            coroutine = StartCoroutine(CustomerWaiting(waitingTime, Leave));
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

    public override void Effect()
    {
        reputation.RemoveReputation(5);
        Leave();
    }
}
