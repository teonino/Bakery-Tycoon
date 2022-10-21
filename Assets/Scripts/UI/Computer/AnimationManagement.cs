using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationManagement : MonoBehaviour {
    [Header("GameObject")]
    [SerializeField] private Button button;
    [Header("Animator")]
    [SerializeField] Animator PriceAnimationTree;
    [Header("Boolean")]
    [SerializeField] private bool isDisplayed = false;

    public void DisplayMenu() {
        if (isDisplayed) {
            PriceAnimationTree.SetTrigger("beingFolded");
            isDisplayed = false;

        }
        else {

            PriceAnimationTree.SetTrigger("beingDisplayed");
            isDisplayed = true;
        }

    }

    /*public void TriggerAnimation()    {
        PriceAnimationTree.SetTrigger("beingFolded");
        isDisplayed = false;
        Debug.Log("Function Triggered");
    }    */
}
