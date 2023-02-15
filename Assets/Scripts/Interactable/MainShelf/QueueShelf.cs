using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueShelf : MonoBehaviour {
    protected AIRandomCustomer customer;

    public void SetCustomer(AIRandomCustomer customer) => this.customer = customer;
    public bool HasCustomer() => customer != null;
    public AIRandomCustomer GetCustomer() => customer;
}
