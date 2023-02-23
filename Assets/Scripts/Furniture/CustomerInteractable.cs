using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomerInteractable : MonoBehaviour
{
    [Tooltip("Debug field")]
    [SerializeField] private AICustomer customer;
    [SerializeField] private Transform targetPosition;
    // Start is called before the first frame update

    public void SetCustomer(AICustomer customer) => this.customer = customer;
    public bool HasCustomer()
    {
        if (customer != null)
            return true;
        else
            return false;
    }
    public Vector3 GetPosition() => targetPosition.position;
    protected abstract void Interactable();
}
