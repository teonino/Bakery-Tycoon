using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Sink : Interactable {

    [SerializeField] private InterractQuest interractQuest;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip SFXClip;
    [SerializeField] private List<ParticleSystem> vfx;

    private void Awake()
    {
        for (int i = 0; i < vfx.Count; i++)
        {
            vfx[i].Stop();
        }
    }

    public override void Effect() {
        if (playerControllerSO.GetPlayerController().GetItemHold() && playerControllerSO.GetPlayerController().GetItemHold().tag == "Plate") {
            interractQuest.OnInterract();
            Addressables.ReleaseInstance(playerControllerSO.GetPlayerController().GetItemHold());
            playerControllerSO.GetPlayerController().SetItemHold(null);
            SFXSource.PlayOneShot(SFXClip);
            for(int i = 0; i < vfx.Count; i++)
            {
                vfx[i].Play();
            }

        }
    }

    public override bool CanInterract() {
        canInterract = playerControllerSO.GetPlayerController().GetItemHold() && playerControllerSO.GetPlayerController().GetItemHold().tag == "Plate";
        return canInterract;
    }
}
