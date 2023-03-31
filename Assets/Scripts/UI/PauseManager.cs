using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private DeliveryManager deliveryManager; //AmafoodScript
    [SerializeField] private RecipeBookManager recipeBookManager;
    [SerializeField] private WorkstationManager workstationManager;

    private void OnEnable()
    {
        if (controller.IsGamepad())
            controller.SetEventSystemToStartButton(resumeButton);
        else
            controller.SetEventSystemToStartButton(null);

        if (deliveryManager.gameObject.activeInHierarchy)
            deliveryManager.LaunchQuitFunction();
        if (recipeBookManager.gameObject.activeSelf)
            recipeBookManager.gameObject.SetActive(false);
        if (workstationManager.gameObject.activeSelf)
            workstationManager.LaunchQuit();

        playerControllerSO.GetPlayerController().playerInput.Pause.Enable();
        Time.timeScale = 0f;
    }

    private void Awake()
    {
        playerControllerSO.GetPlayerController().playerInput.Pause.Unpause.performed += ResumeInput;
    }

    private void ResumeInput(InputAction.CallbackContext context)
    {
        if (context.performed) Resume();
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        playerControllerSO.GetPlayerController().playerInput.Pause.Disable();
        playerControllerSO.GetPlayerController().EnableInput();
    }

    public void MainMenu()
    {
        playerControllerSO.GetPlayerController().playerInput.Pause.Unpause.performed -= ResumeInput;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu_rework");
    }
}
