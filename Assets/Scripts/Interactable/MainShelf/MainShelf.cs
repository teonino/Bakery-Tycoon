using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainShelf : Shelf {
    private List<QueueShelf> queueCustomer;
    private List<QueueBakery> queueCustomerInteracting;


    private new void Start() {
        base.Start();
        queueCustomer = new List<QueueShelf>();

        foreach (Transform child in transform)
            if (child.GetComponent<QueueShelf>())
                queueCustomer.Add(child.GetComponent<QueueShelf>());

        queueCustomerInteracting = new List<QueueBakery>(FindObjectsOfType<QueueBakery>());
    }

    public void GetAvailableQueuePosition(AIRandomCustomer customer) {
        foreach (QueueShelf queuePosition in queueCustomer) {
            if (!queuePosition.HasCustomer() && !customer.inQueue) {
                queuePosition.SetCustomer(customer);
                customer.inQueue = true;
                customer.SetDestination(queuePosition.transform.position);
            }
        }

        if (!customer.inQueue) {
            //ShuffleQueue();
            foreach (QueueBakery queuePosition in queueCustomerInteracting) {
                if (!queuePosition.HasCustomer() && !customer.inQueue) {
                    queuePosition.SetCustomer(customer);
                    customer.inQueue = true;
                    customer.SetInteracting(queuePosition);
                    customer.SetDestination(queuePosition.transform.position);
                }
            }
        }
    }

    public void ForwardQueue(int index) {
        for (int i = index; i < queueCustomer.Count - 1; i++) {
            if (queueCustomer[i + 1].GetCustomer()) {
                queueCustomer[i].SetCustomer(queueCustomer[i + 1].GetCustomer());
                queueCustomer[i + 1].SetCustomer(null);
                queueCustomer[i].GetCustomer().SetDestination(queueCustomer[i].transform.position);
            }
        }

        if (queueCustomerInteracting[0].GetCustomer()) {
            queueCustomer[queueCustomer.Count - 1].SetCustomer(queueCustomerInteracting[0].GetCustomer());
            queueCustomerInteracting[0].SetCustomer(null);
            queueCustomer[queueCustomer.Count - 1].GetCustomer().SetInteracting(null);
            queueCustomer[queueCustomer.Count - 1].GetCustomer().SetDestination(queueCustomer[queueCustomer.Count - 1].transform.position);


            queueCustomer[queueCustomer.Count - 1].GetCustomer().GetAnimator().SetBool("Talk 0", false);
        }

        //set 2nd queueCustomerInteracting place first 
        List<QueueBakery> tmpList = new List<QueueBakery>();

        for (int i = 1; i < queueCustomerInteracting.Count; i++) {
            tmpList.Add(queueCustomerInteracting[i]);
        }
        tmpList.Add(queueCustomerInteracting[0]);

        queueCustomerInteracting = tmpList;
    }

    public void RemoveCustomerInQueue(AICustomer customer) {
        for (int i = 0; i < queueCustomer.Count; i++) {
            if (customer && queueCustomer[i].GetCustomer() == customer) {
                queueCustomer[i].GetCustomer().inQueue = false;
                queueCustomer[i].SetCustomer(null);
                ForwardQueue(i);
            }
        }
    }

    public void PrintQueue() {
        for (int i = 0; i < queueCustomer.Count; i++) {
            if (queueCustomer[i].HasCustomer())
                print(i + " : " + queueCustomer[i].GetCustomer().name);
            else
                print(i + " : null ");
        }
    }

    public bool IsFirstInQueue(AICustomer customer) => queueCustomer[0].GetCustomer() == customer;
}
