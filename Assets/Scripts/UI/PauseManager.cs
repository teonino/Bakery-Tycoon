using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour {
    GameManager gameManager;

    private void OnEnable() {
        Time.timeScale = 0f;
        gameManager.GetPlayerController().playerInput.Pause.Enable();
    }

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.GetPlayerController().playerInput.Pause.Unpause.performed += ResumeInput;
    }

    private void ResumeInput(InputAction.CallbackContext context) {
        if (context.performed) Resume();
    }

    public void Resume() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        gameManager.GetPlayerController().playerInput.Pause.Disable();
        gameManager.GetPlayerController().EnableInput();
    }

    public void Quit() {
        Application.Quit();
    }
}
