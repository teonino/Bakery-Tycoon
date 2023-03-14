using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneBlackScreen : MonoBehaviour
{
    public GameObject blackScreen;
    private Animator blackScreenAnimator;

    private void Awake()
    {
        blackScreenAnimator = blackScreen.GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(Fade());    
    }

    private IEnumerator Fade()
    {
        blackScreenAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(0.585f);
        blackScreen.transform.SetAsFirstSibling();
    }

    public void ReverseFade()
    {
        blackScreen.transform.SetAsLastSibling();
        blackScreenAnimator.SetTrigger("FadeReverse");
    }

}
