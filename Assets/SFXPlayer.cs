using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip interactClip;
    [SerializeField] private AudioSource interactSource;


    public void InteractSound()
    {
        interactSource.PlayOneShot(interactClip);
    }
}
