using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AIRegularCustomer : AICustomer {
    [Header("AI Regular Customer variables")]
    [SerializeField] private AssetReference plateAsset;
    [SerializeField] private int conversationRemaining = 2;
    [SerializeField] private int eatingTime;
    [SerializeField] private InterractQuest onTalk;
    [SerializeField] private RegularSO regularSO;

    [HideInInspector] public Chair chair;
    [HideInInspector] public Table table;

    [SerializeField] ParticleSystem sitDownEffect;
    [SerializeField] ParticleSystem flowerEffect;

    private DialogueManager dialoguePanel;
    private int indexChair;

    public new void InitCustomer() {
        base.InitCustomer();
        day.DayTimeChange += LeaveOnEvening;

        if (!tutorial)
            onTalk = FindObjectOfType<QuestHolder>()?.GetInterractQuest();
        dialoguePanel = FindObjectOfType<DialogueManager>(true);
    }

    new void FixedUpdate() {
        base.FixedUpdate();

        if (chair && state == AIState.idle) {
            Sit();
            if (waitingTime)
                coroutine = StartCoroutine(CustomerWaiting(waitingTime.GetWaitingTime(), Leave));

        }

        //Go to the Chair
        try {
            if (chair && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(chair.transform.position.x, chair.transform.position.z)) < 1 && state == AIState.moving) {
                state = AIState.sitting;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        }
        catch (Exception e) {
            print(e);
        }

        if (table && table.GetItem(false) && state == AIState.sitting) {
            if (table.items[indexChair] && table.items[indexChair].GetComponent<ProductHolder>() && table.items[indexChair].GetComponent<ProductHolder>().product.productSO && table.items[indexChair].GetComponent<ProductHolder>().product.GetName() == requestedProduct.name && table.items[indexChair].GetComponent<ProductHolder>().tag != "Paste") {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                ProductHolder productholder = table.items[indexChair].GetComponent<ProductHolder>();
                if (!item) {
                    if (productholder.product.GetAmount() > 1) {
                        productholder.product.productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            TakeItem(productholder, table.gameObject);
                            table.items[indexChair].GetComponent<ProductHolder>().blocked = true;
                            state = AIState.eating;

                            spawner.RemoveCommandRecap(this);
                            if (tutorial)
                                Leave();
                            else
                                StartCoroutine(CustomerWaiting(eatingTime, Leave));
                        };
                        productholder.product.RemoveAmount();
                    }
                    else {
                        item = table.items[indexChair];
                        TakeItem(item.GetComponent<ProductHolder>(), table.gameObject);
                        productholder.blocked = true;
                        state = AIState.eating;

                        spawner.RemoveCommandRecap(this);
                        if (tutorial)
                            Leave();
                        else
                            StartCoroutine(CustomerWaiting(eatingTime, Leave));
                    }
                }
            }
        }
    }
    public void SetIndexChair(int value) => indexChair = value;


    public override bool isRegular() {
        return true;
    }
    private void Sit() {
        agent.SetDestination(chair.transform.position);
        state = AIState.moving;
    }
    private void LeaveOnEvening() {
        Leave();
        day.DayTimeChange -= LeaveOnEvening;

    }
    protected override void Leave() {
        if (chair)
            chair.ocuppied = false;

        if (state == AIState.eating) {
            plateAsset.InstantiateAsync(table.itemPositions[indexChair].transform).Completed += (go) => {
                go.Result.transform.localPosition = Vector3.zero;
                table.items[indexChair] = go.Result;
            };
            Addressables.ReleaseInstance(table.items[indexChair]);
        }
        else {
            reputation.RemoveReputation(3);
        }

        base.Leave();
    }

    public override void Effect() {
        if (conversationRemaining > 0 && state == AIState.eating) {
            onTalk?.OnInterract();
            dialoguePanel.gameObject.SetActive(true);
            dialoguePanel.GetDialogues(regularSO.GetFriendship(), regularSO.GetName(), regularSO);
            conversationRemaining--;
        }
    }
}
