using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaylistManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicTracks;
    private AudioSource audioSource;
    private int currentTrack = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicTracks[currentTrack];
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentTrack = (currentTrack + 1) % musicTracks.Count;
            audioSource.clip = musicTracks[currentTrack];
            audioSource.Play();
        }
    }
}