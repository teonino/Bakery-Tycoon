using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager_rework : MonoBehaviour
{

    [SerializeField] private GameObject Blackscreen;
    private Animator blackscreenAnimator;

    [SerializeField] private GameObject InputText;
    private Animator inputTextAnimator;

    private void Start()
    {
        blackscreenAnimator = Blackscreen.GetComponent<Animator>();
        inputTextAnimator = InputText.GetComponent<Animator>();
        blackscreenAnimator.SetTrigger("Fade");
        StartCoroutine(SetAtLastSiblingBlackscreen());
    }

    public IEnumerator SetAtLastSiblingBlackscreen()
    {
        yield return new WaitForSeconds(0.3f);
        Blackscreen.transform.SetAsFirstSibling();
        startTextAnim();
    }

    public void startTextAnim()
    {
        inputTextAnimator.SetTrigger("Unfade");
    }

}
