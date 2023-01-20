using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerControllerSO;

    private void OnEnable() {
        Time.timeScale = 0f;
        playerControllerSO.GetPlayerController().playerInput.Pause.Enable();
        if (controller.IsGamepad())
            controller.SetEventSystemToStartButton(resumeButton);
        else
            controller.SetEventSystemToStartButton(null);
    }

    private void Awake() {
        playerControllerSO.GetPlayerController().playerInput.Pause.Unpause.performed += ResumeInput;
    }

    private void ResumeInput(InputAction.CallbackContext context) {
        if (context.performed) Resume();
    }

    public void Resume() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        playerControllerSO.GetPlayerController().playerInput.Pause.Disable();
        playerControllerSO.GetPlayerController().EnableInput();
    }

    public void MainMenu() {
        playerControllerSO.GetPlayerController().playerInput.Pause.Unpause.performed -= ResumeInput;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
