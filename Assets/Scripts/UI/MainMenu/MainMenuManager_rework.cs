using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager_rework : MonoBehaviour
{

    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    public Controller GetController() => controller;
    [HideInInspector] public PlayerInput playerInput { get; private set; }


    [SerializeField] private GameObject Blackscreen;
    private Animator blackscreenAnimator;

    [SerializeField] private GameObject InputText;
    private Animator inputTextAnimator;

    [SerializeField] private GameObject logo;
    private Animator logoAnimator;

    [SerializeField] private GameObject buttonPanel;
    private Animator buttonPanelAnimator;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.UI.Enable();
        playerInput.UI.AnyKeyPressed.performed += LaunchAnyKeyPressed;
    }

    private void Start()
    {
        blackscreenAnimator = Blackscreen.GetComponent<Animator>();
        inputTextAnimator = InputText.GetComponent<Animator>();
        logoAnimator = logo.GetComponent<Animator>();
        buttonPanelAnimator = buttonPanel.GetComponent<Animator>();
        blackscreenAnimator.SetTrigger("Fade");
        StartCoroutine(SetAtLastSiblingBlackscreen());
    }

    public IEnumerator SetAtLastSiblingBlackscreen()
    {
        yield return new WaitForSeconds(0.3f);
        Blackscreen.transform.SetAsFirstSibling();
        startLogoAnim();
        yield return new WaitForSeconds(0.8f);
        startTextAnim();
    }

    public void startLogoAnim()
    {
        logoAnimator.SetTrigger("FadeToUnfade");
    }

    public void startTextAnim()
    {
        inputTextAnimator.SetTrigger("Unfade");
    }

    private void LaunchAnyKeyPressed(InputAction.CallbackContext context)
    {
        print("any key pressed");
        StartCoroutine(AnyKeyPressed());
    }

    private IEnumerator AnyKeyPressed()
    {
        inputTextAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        logoAnimator.SetTrigger("transitionToCornerUpLeft");
        yield return new WaitForSeconds(1);
        buttonPanelAnimator.SetTrigger("OutsideToInside");
    }
    private void OnDisable()
    {
        playerInput.UI.AnyKeyPressed.performed -= LaunchAnyKeyPressed;
        playerInput.UI.Disable();
    }

}
