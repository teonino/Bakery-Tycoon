using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBakery : QueueShelf
{
    [SerializeField] private Animator animator;

    public void Interact() {
        animator.SetTrigger("Sit");
    }
}
