using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX", menuName = "Data/SFX")]
public class SFX_SO : ScriptableObject
{
    [SerializeField] private AudioClip notificationSFX;
    [SerializeField] private AudioClip questSFX;
    [SerializeField] private AudioClip moneySFX;
    [SerializeField] private AudioClip reputationSFX;
    [SerializeField] private AudioClip minigameCriticSuccessSFX;
    [SerializeField] private AudioClip minigameSuccessSFX;
    [SerializeField] private AudioClip minigameFailureSFX;

    public Action<AudioClip> action;
    public void Invoke(string key)
    {
        AudioClip clip = null;
        switch(key)
        {
            case "Notification":
                clip = notificationSFX;
                break;
            case "Quest":
                clip = questSFX;
                break;
            case "Money":
                clip = moneySFX;
                break;
        }

        action.Invoke(clip);
    }
}
