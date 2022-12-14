using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
    [SerializeField] private PlayerControllerSO playerControllerSO;
    private void Update() {
        if (gameObject.activeSelf && playerControllerSO.GetPlayerInputState())
            playerControllerSO.GetPlayerController().DisableInput();
    }
}
