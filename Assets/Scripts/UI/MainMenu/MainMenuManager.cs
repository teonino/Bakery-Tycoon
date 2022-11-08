using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Screen = new List<GameObject>();
    [SerializeField] private List<Animator> ScreenAnimator = new List<Animator>();
    [SerializeField] private List<GameObject> Button = new List<GameObject>();
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private bool pressAnyButton = false;

    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.UI.Enable();
        playerInput.UI.AnyKeyPressed.performed += Continue;
        playerInput.UI.Click.performed += Continue;
        ScreenAnimator[0].SetTrigger("TriggerFade");
        StartCoroutine(DisplayTitle());
        StartCoroutine(WaitingTimeToDisableBlackscreen());
        StartCoroutine(DisplayLogo());
    }

    private void Update()
    {

    }

    void Continue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(AnimationLogo());
        }
    }

    private IEnumerator AnimationLogo()
    {
        if (pressAnyButton)
        {
            ScreenAnimator[1].SetTrigger("TriggerMovement");
            ScreenAnimator[2].SetTrigger("TriggerHide");
            yield return new WaitForSeconds(3);
            StartCoroutine(DisplayButton());
            pressAnyButton = false;
        }
        else
        {
            print("Not this time");
        }


    }

    private IEnumerator DisplayButton()
    {
        Button[0].SetActive(true);
        Button[1].SetActive(true);
        Button[2].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        ScreenAnimator[3].SetTrigger("Spawn");
        ScreenAnimator[4].SetTrigger("Spawn");
        ScreenAnimator[5].SetTrigger("Spawn");

    }

    private IEnumerator DisplayLogo()
    {
        yield return new WaitForSeconds(1);
        Screen[1].SetActive(true);
        ScreenAnimator[1].SetTrigger("TriggerFade");
        yield return new WaitForSeconds(0.5f);
        pressAnyButton = true;
        Screen[2].SetActive(true);
        ScreenAnimator[2].SetTrigger("TriggerDisplay");
    }

    private IEnumerator DisplayTitle()
    {
        yield return new WaitForSeconds(1);
    }

    private IEnumerator WaitingTimeToDisableBlackscreen()
    {
        yield return new WaitForSeconds(3f);
        DisableBlackScreen();
    }

    private void DisableBlackScreen()
    {
        Screen[0].SetActive(false);
    }
}
