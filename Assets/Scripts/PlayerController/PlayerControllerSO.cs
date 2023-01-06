using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerSO", menuName = "Data/PlayerControllerSO")]
public class PlayerControllerSO : ScriptableObject
{
    private PlayerController playerController;

    public PlayerController GetPlayerController() => playerController;
    public void SetPlayerController(PlayerController playerController) => this.playerController = playerController;
    public bool GetPlayerInputState() => playerController.GetPlayerInputEnabled();
}
