using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainShelf : Shelf {
    private List<QueueShelf> queueCustomer;
    private List<QueueShelf> queueCustomerInteracting;

    private new void Start() {
        base.Start();
        queueCustomer = new List<QueueShelf>();
        queueCustomerInteracting = new List<QueueShelf>();

        foreach (Transform child in transform)
            if (child.GetComponent<QueueShelf>())
                queueCustomer.Add(child.GetComponent<QueueShelf>());

        QueueShelf[] list = FindObjectsOfType<QueueShelf>();
        foreach (QueueShelf shelf in list) {
            if (!queueCustomer.Contains(shelf))
                queueCustomerInteracting.Add(shelf);
        }
    }

    public void GetAvailableQueuePosition(AIRandomCustomer customer) {
        foreach (QueueShelf queuePosition in queueCustomer) {
            if (!queuePosition.customer && !customer.inQueue) {
                queuePosition.customer = customer;
                customer.inQueue = true;
                customer.SetDestination(queuePosition.transform.position);
            }
        }

        if (!customer.inQueue) {
            //ShuffleQueue();
            foreach (QueueShelf queuePosition in queueCustomerInteracting) {
                if (!queuePosition.customer && !customer.inQueue) {
                    queuePosition.customer = customer;
                    customer.inQueue = true;
                    customer.SetDestination(queuePosition.transform.position);
                }
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

        if (queueCustomerInteracting[0].customer) {
            queueCustomer[queueCustomer.Count - 1].customer = queueCustomerInteracting[0].customer;
            queueCustomerInteracting[0].customer = null;
            queueCustomer[queueCustomer.Count - 1].customer.SetDestination(queueCustomer[queueCustomer.Count - 1].transform.position);
        }

        //set 2nd queueCustomerInteracting place first 
        List<QueueShelf> tmpList = new List<QueueShelf>();

        for (int i = 1; i < queueCustomerInteracting.Count; i++) {
            tmpList.Add(queueCustomerInteracting[i]);
        }
        tmpList.Add(queueCustomerInteracting[0]);

        queueCustomerInteracting = tmpList;
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
    private void ShuffleQueue() {
        int n = queueCustomerInteracting.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n);
            QueueShelf value = queueCustomerInteracting[k];
            queueCustomerInteracting[k] = queueCustomerInteracting[n];
            queueCustomerInteracting[n] = value;
        }
    }

    public bool IsFirstInQueue(AICustomer customer) => queueCustomer[0].customer == customer;
}
