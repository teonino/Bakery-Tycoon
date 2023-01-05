using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Button = new List<GameObject>();
    [SerializeField] private ChangeMenu InternalMenu;
    [SerializeField] private GameObject Blackscreen;
    private Animator blackscreenAnimator;

    private void Start()
    {
        blackscreenAnimator = Blackscreen.GetComponent<Animator>();
        blackscreenAnimator.SetTrigger("Fade");
    }


    public void displayMainMenuTools()
    {
        StartCoroutine(DisplayButton());
    }


    public IEnumerator DisplayButton()
    {
        Button[0].SetActive(true);
        Button[1].SetActive(true);
        Button[2].SetActive(true);
        yield return new WaitForSeconds(0.3f);


    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public IEnumerator BlackscreenTransition(string SceneToLoad)
    {
        blackscreenAnimator.SetTrigger("FadeReverse");
        yield return new WaitForSeconds(0.45f);
        print("After delay");
        if(SceneToLoad != "quit")
        {
            SceneManager.LoadScene(SceneToLoad);
        }
        else
        {
            Application.Quit();
        }
    }

    public IEnumerator BlackscreenTransitionInMainMenu(int MenuToSwitch)
    {
        blackscreenAnimator.SetTrigger("FadeReverse");
        yield return new WaitForSeconds(0.25f);
        blackscreenAnimator.SetTrigger("Fade");
        
        for (int i = 0; i < InternalMenu.Panel.Count; i++)
        {
            InternalMenu.Panel[i].SetActive(false);
        }

        InternalMenu.Panel[MenuToSwitch].SetActive(true);
        InternalMenu.Panel[MenuToSwitch].transform.SetAsFirstSibling();
    }

}
