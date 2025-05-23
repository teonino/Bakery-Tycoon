using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AIRegularCustomer : AICustomer
{
    [Header("AI Regular Customer variables")]
    [SerializeField] private AssetReference plateAsset;
    [SerializeField] private int conversationRemaining = 2;
    [SerializeField] private int eatingTime;
    [SerializeField] private InterractQuest onTalk;
    [SerializeField] private RegularSO regularSO;

    [HideInInspector] public Chair chair;
    [HideInInspector] public Table table;

    [SerializeField] private ParticleSystem sitDownEffect;
    [SerializeField] private ParticleSystem flowerEffect;

    private DialogueManager dialoguePanel;
    private int indexChair;

    public Action addConversation;


    protected override void Awake()
    {
        base.Awake();
        sitDownEffect.Stop();
        flowerEffect.Stop();
    }

    public new void InitCustomer()
    {
        base.InitCustomer();

        FindObjectOfType<MainShelf>().SetDestinationToPos(this);
        regularSO = FindObjectOfType<RegularHolder>().GetRegular(regularSO);
        animator.SetTrigger("Walk");
        day.DayTimeChange += LeaveOnEvening;
        if (!tutorial)
            onTalk = FindObjectOfType<QuestHolder>()?.GetInterractQuest();
        dialoguePanel = FindObjectOfType<DialogueManager>(true);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (chair && state == AIState.idle)
        {
            if (waitingTime && coroutine == null)
            {
                coroutine = StartCoroutine(CustomerWaiting(waitingTime.GetWaitingTime(), Leave));
                flowerEffect.Play();
            }
        }

        //Go to the Chair
        if (chair && state == AIState.moving && Vector3.Distance(transform.position, chair.transform.position) < 1)
        {
            animator.SetTrigger("Sit");
            sitDownEffect.Play();
            productCanvas.SetActive(true);
            state = AIState.sitting;
            transform.LookAt(table.transform.GetChild(0).transform.position);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }

        if (table && table.GetItem(false) && state == AIState.sitting)
        {
            if (table.items[indexChair] && table.items[indexChair].GetComponent<ProductHolder>() && table.items[indexChair].GetComponent<ProductHolder>().product.productSO && table.items[indexChair].GetComponent<ProductHolder>().product.GetKeyName() == requestedProduct.keyName && table.items[indexChair].GetComponent<ProductHolder>().tag != "Paste")
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                ProductHolder productholder = table.items[indexChair].GetComponent<ProductHolder>();
                if (!item)
                {
                    if (productholder.product.GetAmount() > 1)
                    {
                        productholder.product.productSO.asset.InstantiateAsync(transform).Completed += (go) =>
                        {
                            item = go.Result;
                            TakeItem(productholder, table.gameObject);
                            table.items[indexChair].GetComponent<ProductHolder>().blocked = true;
                            state = AIState.eating;

                            spawner.RemoveCommandRecap(this);
                            if (tutorial)
                                Leave();
                            else
                            {
                                StartCoroutine(CustomerWaiting(eatingTime, Leave));
                                animator.SetTrigger("Served");
                                flowerEffect.Play();
                            }
                        };
                        productholder.product.RemoveAmount();
                    }
                    else
                    {
                        item = table.items[indexChair];
                        TakeItem(item.GetComponent<ProductHolder>(), table.gameObject);
                        productholder.blocked = true;
                        state = AIState.eating;

                        spawner.RemoveCommandRecap(this);
                        if (tutorial)
                            Leave();
                        else
                        {
                            StartCoroutine(CustomerWaiting(eatingTime, Leave));
                            animator.SetTrigger("Served");
                            flowerEffect.Play();
                        }
                    }
                }
            }
        }
    }
    public void SetIndexChair(int value) => indexChair = value;


    public override bool isRegular()
    {
        return true;
    }
    public void GoToChair()
    {
        agent.SetDestination(chair.transform.position);
        state = AIState.moving;
    }
    private void LeaveOnEvening()
    {
        Leave();
        day.DayTimeChange -= LeaveOnEvening;

    }

    private bool waitAnimation = false;

    protected override void Leave()
    {
        if (waitAnimation && item)
        {
            StartCoroutine(waitEndOfAnimation());
        }
        else
        {
            if (chair)
                chair.ocuppied = false;

            if (state == AIState.eating)
            {
                plateAsset.InstantiateAsync(table.itemPositions[indexChair].transform).Completed += (go) =>
                {
                    go.Result.transform.localPosition = Vector3.zero;
                    table.items[indexChair] = go.Result;
                };
                Addressables.ReleaseInstance(table.items[indexChair]);
            }
            else
            {
                animator.SetTrigger("End Sit");
                reputation.RemoveReputation(3);
            }
            base.Leave();
        }
    }

    private IEnumerator waitEndOfAnimation()
    {
        yield return new WaitForSeconds(5.4f);
        waitAnimation = false;
        Leave();
    }

    public override void Effect()
    {
        if (conversationRemaining > 0 && state == AIState.eating)
        {
            onTalk?.OnInterract();
            dialoguePanel.gameObject.SetActive(true);
            dialoguePanel.GetDialogues(regularSO.GetFriendship(), regularSO.GetName(), regularSO);
            conversationRemaining--;
        }
    }
}
