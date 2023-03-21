using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : QueueBakery
{
    [SerializeField] private Transform focusedItem;
    public override void Interact(Animator animator)
    {
        animator.SetTrigger(animationKey);
        customer.transform.LookAt(focusedItem.position);
    }
}
