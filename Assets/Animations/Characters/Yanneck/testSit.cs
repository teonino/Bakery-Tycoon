using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSit : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {      
        StartCoroutine(SitDown());
    }

    IEnumerator SitDown()
    {
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("Sit");
    }
}
