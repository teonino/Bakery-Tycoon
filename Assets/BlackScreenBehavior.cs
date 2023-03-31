using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenBehavior : MonoBehaviour
{
    [SerializeField] private GameObject blackScreen;
    private Animator blackScreenAnimator;

    private void Awake()
    {
        blackScreenAnimator = blackScreen.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(BlackscreenStartupFade());
    }

    private IEnumerator BlackscreenStartupFade()
    {
        blackScreen.transform.SetAsLastSibling();
        blackScreenAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        blackScreen.transform.SetAsFirstSibling();
    }

    internal void BlackScreenFade()
    {
        blackScreen.transform.SetAsLastSibling();
        blackScreenAnimator.SetTrigger("FadeReverse");
    }

}
