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
        blackScreenAnimator.SetTrigger("Fade");
    }

    public void ReverseFade()
    {
        blackScreenAnimator.SetTrigger("FadeReverse");
    }

}
