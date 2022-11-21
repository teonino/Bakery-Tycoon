using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentCanvasManager : MonoBehaviour
{
    public int timeDisplaying = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifespan());
    }

    private IEnumerator Lifespan() {
        yield return new WaitForSeconds(timeDisplaying);
        Destroy(gameObject);
    }
}
