using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggeranim : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        StartCoroutine(WaitToAnim());
    }


    IEnumerator WaitToAnim()
    {
        yield return new WaitForSeconds(3.5f);
        animator.SetTrigger("hello");
    }
}
