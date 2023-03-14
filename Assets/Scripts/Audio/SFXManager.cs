using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    [SerializeField] private SFX_SO sfxSO;
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        sfxSO.action += Play;
    }

    private void Play(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnDestroy() {
        sfxSO.action -= Play;
    }
}
