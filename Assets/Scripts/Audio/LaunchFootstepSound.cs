using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchFootstepSound : MonoBehaviour
{

    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource footstepSource;
    // Start is called before the first frame update

    public void FootstepSound()
    {
        footstepSource.PlayOneShot(footstepClip);
    }

}
