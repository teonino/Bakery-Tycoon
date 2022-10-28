using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Shelf : Interactable {
    public GameObject item;
    private List<QueueShelf> queueCustomer;
    public GameObject itemPosition;

    private void Start() {
        queueCustomer = new List<QueueShelf>();
        foreach (Transform child in transform)
            if (child.GetComponent<QueueShelf>())
                queueCustomer.Add(child.GetComponent<QueueShelf>());
    }

    public void GetAvailableQueuePosition(AICustomer customer) {
        foreach (QueueShelf queuePosition in queueCustomer) {
            if (!queuePosition.customer && !customer.inQueue) {
                queuePosition.customer = customer;
                queuePosition.customer.inQueue = true;
                queuePosition.customer.SetDestination(queuePosition.transform.position);
            }
        }
    }

    public void ForwardQueue(int index) {
        for (int i = index; i < queueCustomer.Count - 1; i++) {
            if (queueCustomer[i + 1].customer) {
                queueCustomer[i].customer = queueCustomer[i + 1].customer;
                queueCustomer[i + 1].customer = null;
                queueCustomer[i].customer.SetDestination(queueCustomer[i].transform.position);
            }
        }
        queueCustomer[queueCustomer.Count - 1].customer = null;
    }

    public void RemoveCustomerInQueue(AICustomer customer) {
        for (int i = 0; i < queueCustomer.Count; i++) {
            if (queueCustomer[i].customer == customer) {
                queueCustomer[i].customer.inQueue = false;
                queueCustomer[i].customer = null; ;
                ForwardQueue(i);
            }
        }
    }

    public void PrintQueue() {
        for (int i = 0; i < queueCustomer.Count; i++) {
            if (queueCustomer[i].customer)
                print(i + " : " + queueCustomer[i].customer.name);
            else
                print(i + " : null ");
        }
    }

    public bool IsFirstInQueue(AICustomer customer) => queueCustomer[0].customer == customer;
    public override void Effect() {
        if (playerController.itemHolded && item == null) {
            playerController.itemHolded.transform.SetParent(transform);
            item = playerController.itemHolded;
            playerController.itemHolded = null;
            item.transform.localPosition = itemPosition.transform.localPosition;
        }
        else if (!playerController.itemHolded && item) {
            Transform arm = playerController.gameObject.transform.GetChild(0);

            playerController.itemHolded = item;
            item = null;
            playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
            playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
        }
    }
}
