using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class creditScrolling : MonoBehaviour
{
    private MainMenuManager_rework manager;
    private bool creditStart;

    private float t;

    private void OnEnable()
    {
        manager = GetComponentInParent<MainMenuManager_rework>();
        creditStart = true;
        StartCoroutine(waitfordestroy());
    }

    private void Update()
    {
        if (creditStart)
        {
            gameObject.transform.Translate(Vector3.up * manager.creditSpeed);
        }
    }

    private IEnumerator waitfordestroy()
    {
        yield return new WaitForSeconds(76);
        manager.creditIndex = 0;
        manager.isInCredit = false;
        Destroy(this.gameObject);
    }

    public IEnumerator waitForDestroyAfterCredits()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
        manager.isInCredit = false;
    }

}


