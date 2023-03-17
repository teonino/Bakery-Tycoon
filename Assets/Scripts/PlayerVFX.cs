using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{

    private int indexVFX = 0;

    [Header("VFX")]
    [SerializeField] private ParticleSystem leftVFX;
    [SerializeField] private ParticleSystem rightVFX;

    private void vfxStartup()
    {
        if (indexVFX == 0)
        {
            leftVFX.Play();
            indexVFX++;
        }
        else
        {
            rightVFX.Play();
            indexVFX = 0;
        }
    }

    private void Awake()
    {
        leftVFX.Stop();
        rightVFX.Stop();
    }

}
