using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AIRegularCustomer : AICustomer {

    [SerializeField] private AssetReference dialoguePanelAsset;
    [SerializeField] private AssetReference plateAsset;

    public Chair chair;
    public int indexChair;
    public Table table;
    private int conversationRemaining = 2;

    new void Awake() {
        base.Awake();
    }

    new void FixedUpdate() {
        base.FixedUpdate();

        if (chair && state == AIState.idle) {
            Sit();
        }

        //Go to the Chair
        try {
            if (chair && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(chair.transform.position.x, chair.transform.position.z)) < 1 && state == AIState.moving) {
                state = AIState.sitting;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                waitingCoroutine = StartCoroutine(CustomerWaiting(waitingTime, Leave));
            }
        } catch (Exception e) {
            print(e);
        }

        if (table && table.GetItem(false) && state == AIState.sitting) {
            if (table.items[indexChair] && table.items[indexChair].GetComponent<ProductHolder>() && table.items[indexChair].GetComponent<ProductHolder>().product.productSO && table.items[indexChair].GetComponent<ProductHolder>().product.GetName() == requestedProduct.name && table.items[indexChair].GetComponent<ProductHolder>().tag != "Paste") {
                StopCoroutine(waitingCoroutine);
                //Take item
                if (!item) {
                    if (table.items[indexChair].GetComponent<ProductHolder>().product.amount > 1) {
                        table.items[indexChair].GetComponent<ProductHolder>().product.productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            TakeItem(table.items[indexChair].GetComponent<ProductHolder>(), table.gameObject);
                            table.items[indexChair].GetComponent<ProductHolder>().blocked = true;
                            StartCoroutine(CustomerWaiting(waitingTime * 2, Leave));
                            state = AIState.eating;
                        };
                        table.items[indexChair].GetComponent<ProductHolder>().product.amount--;
                    }
                    else {
                        item = table.items[indexChair];
                        TakeItem(item.GetComponent<ProductHolder>(), table.gameObject);
                        table.items[indexChair].GetComponent<ProductHolder>().blocked = true;
                        StartCoroutine(CustomerWaiting(waitingTime * 2, Leave));
                        state = AIState.eating;
                    }
                }
            }
        }
    }

    private void Sit() {
        agent.SetDestination(chair.transform.position);
        state = AIState.moving;
    }

    private new void Leave() { 
        if (chair)
            chair.ocuppied = false;

        if (state == AIState.eating) {
            plateAsset.InstantiateAsync(table.itemPositions[indexChair].transform).Completed += (go) => {
                go.Result.transform.localPosition = Vector3.zero;
                table.items[indexChair] = go.Result;
            };
            Addressables.ReleaseInstance(table.items[indexChair]);
        } else {
            gameManager.RemoveReputation(3);
        }

        base.Leave();
    }

    public override void Effect() {
        if (conversationRemaining > 0 && state == AIState.eating) {
            gameManager.GetPlayerController().DisableInput();
            dialoguePanelAsset.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) =>
                go.Result.GetComponent<DialogueManager>().GetDialogues(1);
            Time.timeScale = 0;
            conversationRemaining--;
        }
    }
}
