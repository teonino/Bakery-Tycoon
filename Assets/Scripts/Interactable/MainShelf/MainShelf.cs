using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainShelf : Shelf {
    private List<QueueShelf> queueCustomer;

    private new void Start() {
        base.Start();
        queueCustomer = new List<QueueShelf>();
        foreach (Transform child in transform)
            if (child.GetComponent<QueueShelf>())
                queueCustomer.Add(child.GetComponent<QueueShelf>());
    }

    public void GetAvailableQueuePosition(AIRandomCustomer customer) {
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
            if (customer && queueCustomer[i].customer == customer) {
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
}
