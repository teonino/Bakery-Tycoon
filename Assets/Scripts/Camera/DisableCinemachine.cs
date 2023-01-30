using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DisableCinemachine : MonoBehaviour
{
    [SerializeField] private List<GameObject> Camera;
    [SerializeField] private Transform Player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            print("Disabled");
            StartCoroutine(TimeToDisableCinemachine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Enable");
            for (int i = 0; i < Camera.Count; i++)
            {
                Camera[i].GetComponent<CinemachineFreeLook>().LookAt = Player;
                Camera[i].GetComponent<CinemachineFreeLook>().Follow = Player;
            }
        }
    }

    IEnumerator TimeToDisableCinemachine()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Camera.Count; i++)
        {
            Camera[i].GetComponent<CinemachineFreeLook>().LookAt = null;
            Camera[i].GetComponent<CinemachineFreeLook>().Follow = null;
            print("Camera Disabled");
        }
    }

}
