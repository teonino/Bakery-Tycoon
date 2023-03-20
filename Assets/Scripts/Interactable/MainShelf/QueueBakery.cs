using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBakery : QueueShelf
{
    [SerializeField] protected string animationKey;

    protected bool customerHere = false;
    public virtual void Interact(Animator animator) {
        animator.SetTrigger(animationKey);
    }

    public bool IsCustomerHere() => customerHere;
}