using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [SerializeField] private GameObject player;
    private Animator playerAnimator;

    [SerializeField] private GameObject CustomerSpawn;
    private bool canPressInput = true;
    public MainMenuCharacter currentCustomer;
    public Animator currentCustomerAnimator;
    [SerializeField] private List<MainMenuCharacter> mainMenuCharacters = new List<MainMenuCharacter>();
    [SerializeField] private List<GameObject> panelMainMenu = new List<GameObject> ();
    [SerializeField] private List<Animator> animators = new List<Animator> ();
    [SerializeField] private GameObject currentPanel;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.UI.Enable();
        playerInput.UI.AnyKeyPressed.performed += LaunchAnyKeyPressed;

        for (int i = 0; i < panelMainMenu.Count; i++)
        {
            animators.Add(panelMainMenu[i].GetComponent<Animator>());
        }
        currentPanel = panelMainMenu[0];
    }

    private void Start()
    {
        blackscreenAnimator = Blackscreen.GetComponent<Animator>();
        inputTextAnimator = InputText.GetComponent<Animator>();
        logoAnimator = logo.GetComponent<Animator>();
        buttonPanelAnimator = buttonPanel.GetComponent<Animator>();
        blackscreenAnimator.SetTrigger("Fade");
        playerAnimator = player.GetComponent<Animator>();
        StartCoroutine(SetAtLastSiblingBlackscreen());
        StartCoroutine(spawnCustomer());
    }

    public IEnumerator SetAtLastSiblingBlackscreen()
    {
        yield return new WaitForSeconds(0.3f);
        Blackscreen.transform.SetAsFirstSibling();
        startLogoAnim();
        yield return new WaitForSeconds(0.7f);
        startTextAnim();
        canPressInput = true;
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
        if (canPressInput)
        {
            print("any key pressed");
            canPressInput = false;
            StartCoroutine(AnyKeyPressed());
        }
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

    public IEnumerator spawnCustomer()
    {
        if (currentCustomer == null)
        {
            int randomSpawnTime = Random.Range(1, 2);
            yield return new WaitForSeconds(randomSpawnTime);
            int rdm = Random.Range(0, mainMenuCharacters.Count);
            currentCustomer = mainMenuCharacters[rdm];
            currentCustomerAnimator = currentCustomer.GetComponentInChildren<Animator>();
            Instantiate(currentCustomer, CustomerSpawn.transform);
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();
        }
    }

    public void LaunchDisplayPanel(string panelName)
    {
        StartCoroutine(DisplayPanel(panelName));
    }

    public void ActiveCustomer()
    {
        currentCustomer.gameObject.SetActive(true);
    }

    public void LaunchLeavingFunction()
    {
        print("launch leaving function");
        StartCoroutine(currentCustomer.Leaving());
    }

    public IEnumerator DisplayPanel(string panelName)
    {
        Animator currentPanelAnimator = currentPanel.GetComponent<Animator>();
        if(panelName == "Options")
        {
            currentPanelAnimator.SetTrigger("InsideToOutside");
            yield return new WaitForSeconds(1);
            panelMainMenu[1].SetActive(true);
            panelMainMenu[2].SetActive(false);
            currentPanel = panelMainMenu[1];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.SetTrigger("OutsideToInside");
        }
        else if (panelName == "Credits")
        {
            currentPanelAnimator.SetTrigger("InsideToOutside");
            yield return new WaitForSeconds(1);
            panelMainMenu[1].SetActive(false);
            panelMainMenu[2].SetActive(true);
            currentPanel = panelMainMenu[2];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.SetTrigger("OutsideToInside");
        }
        else if (panelName == "Tutorial")
        {
            Blackscreen.transform.SetAsLastSibling();
            blackscreenAnimator.SetTrigger("FadeReverse");
            yield return new WaitForSeconds(0.7f);
            SceneManager.LoadScene("Tutorial");
        }
        else if (panelName == "Quit")
        {
            Blackscreen.transform.SetAsLastSibling();
            blackscreenAnimator.SetTrigger("FadeReverse");
            yield return new WaitForSeconds(0.7f);
            Application.Quit();
        }
        else if (panelName == "Play")
        {
            Blackscreen.transform.SetAsLastSibling();
            blackscreenAnimator.SetTrigger("FadeReverse");
            yield return new WaitForSeconds(0.7f);
            SceneManager.LoadScene("FirstBakery_New");
        }
        else if (panelName == "Back")
        {
            currentPanelAnimator.SetTrigger("InsideToOutside");
            yield return new WaitForSeconds(1);
            currentPanel = panelMainMenu[0];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.SetTrigger("OutsideToInside");
        }
    }

}
