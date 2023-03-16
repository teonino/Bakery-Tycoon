using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteractions : MonoBehaviour
{
    public AIRandomCustomer customer;

    public void randomTalking() {
        int rng = Random.Range(0, 3);

        switch (rng) {
            case 0:
                customer.GetAnimator().SetTrigger("Talk");
                break;
            case 1:
                customer.GetAnimator().SetTrigger("Listen");
                break;
            case 2:
                customer.GetAnimator().SetTrigger("Agree");
                break;
        }

    }
}
