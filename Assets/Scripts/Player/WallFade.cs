using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour {

    [SerializeField] private GameObject wallToDispawn;
    [SerializeField][Range(5f, 15f)] float time;
    private float lerpTime;
    public bool thisRoomIsActive;
    private List<Coroutine> invisibleCoroutines;
    private List<Coroutine> visibleCoroutines;
    [SerializeField] private bool stillInUse = false;

    private void Start() {
        lerpTime = time * Time.deltaTime;
        invisibleCoroutines = new List<Coroutine>();
        visibleCoroutines = new List<Coroutine>();
    }

    public void DisableWall()
    {
        if (thisRoomIsActive) {
                if (stillInUse)
                {

                for (int i = 0; i < wallToDispawn.transform.childCount; i++)
                {
                    StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), 0));
                    //wallToDispawn.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
                }

            }
        }
    }

    public void EnableWall()
    { 
    
            if (stillInUse)
            {

            for (int i = 0; i < wallToDispawn.transform.childCount; i++)
            {
                StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), 1));
                //wallToDispawn.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

        }
    }

    private IEnumerator ChangeColor(Transform go, float opacity) {
        while (go.GetComponent<Renderer>().material.GetFloat("_AlphaStrenght") != opacity) {
            float a = go.GetComponent<Renderer>().material.GetFloat("_AlphaStrenght");
            go.GetComponent<Renderer>().material.SetFloat("_AlphaStrenght", Mathf.Lerp(a, opacity, lerpTime));
            print(opacity);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
