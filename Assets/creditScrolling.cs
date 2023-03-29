using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditScrolling : MonoBehaviour
{
    private MainMenuManager_rework manager;
    private bool creditStart;

    private void OnEnable()
    {
        manager = GetComponentInParent<MainMenuManager_rework>();
        creditStart = true;
        StartCoroutine(waitfordestroy());
    }

    private void Update()
    {
        if(creditStart)
        {
            gameObject.transform.Translate(Vector3.up * manager.creditSpeed);
        }
    }

    private IEnumerator waitfordestroy()
    {
        yield return new WaitForSeconds(60);
        manager.creditIndex = 0;
        Destroy(this.gameObject);
    }
}

