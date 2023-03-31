using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : QueueBakery
{
    private Rigidbody rb;
    private RigidbodyConstraints tmp;

    [SerializeField] private Transform focusedItem;
    public override void Interact(Animator animator)
    {
        animator.SetTrigger(animationKey);
        customer.transform.LookAt(focusedItem.position);

        rb = customer.GetComponent<Rigidbody>();
        tmp = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        rb.constraints = tmp;
    }
}
