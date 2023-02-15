using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePosition : QueueBakery {
    [SerializeField] private Transform focusedItem;
    [SerializeField] private DialoguePosition secondPosition;
    [SerializeField] private string idleAnimation;

    public override void Interact(Animator animator) {
        customerHere = true;

        if (HasCustomer() && secondPosition.HasCustomer() && secondPosition.IsCustomerHere()) {
            customer.GetAnimator().SetTrigger("Idle");
            secondPosition.GetCustomer().GetAnimator().SetTrigger("Idle");

            customer.GetAnimator().SetBool("Talk 0", true);
            secondPosition.GetCustomer().GetAnimator().SetBool("Talk 0", true);


            customer.GetComponentInChildren<TalkInteractions>().customer = customer;
            secondPosition.GetCustomer().GetComponentInChildren<TalkInteractions>().customer = secondPosition.GetCustomer();

            customer.GetComponentInChildren<TalkInteractions>().randomTalking();
            secondPosition.GetCustomer().GetComponentInChildren<TalkInteractions>().randomTalking();
        }
        else if (HasCustomer() && !secondPosition.HasCustomer())
            animator.SetTrigger(idleAnimation);
        else if (!HasCustomer() && secondPosition.HasCustomer())
            secondPosition.GetCustomer().GetAnimator().SetTrigger(idleAnimation);


        customer.transform.LookAt(focusedItem.position);
    }
}
