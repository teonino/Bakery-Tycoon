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

    private void OnEnable()
    {
        playerInput.UI.AnyKeyPressed.performed += LaunchAnyKeyPressed;
    }

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

    private void LaunchAnyKeyPressed(InputAction.CallbackContext context)
    {
        StartCoroutine(AnyKeyPressed());
    }

    private IEnumerator AnyKeyPressed()
    {
        inputTextAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(5f);
    }

}
