using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerSO", menuName = "Data/PlayerControllerSO")]
public class PlayerControllerSO : ScriptableObject
{
    [SerializeField] PlayerController playerController;

    public PlayerController GetPlayerController() => playerController;
    public void SetPlayerController(PlayerController playerController) => this.playerController = playerController;
}
