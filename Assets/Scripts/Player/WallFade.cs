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
    [SerializeField] private float actualOpacity;

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
                    actualOpacity = wallToDispawn.transform.GetChild(i).GetComponent<Renderer>().material.GetFloat("_Opacity");
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
                actualOpacity = wallToDispawn.transform.GetChild(i).GetComponent<Renderer>().material.GetFloat("_Opacity");
                StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), 1));
                //wallToDispawn.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
            }

        }
    }

    private IEnumerator ChangeColor(Transform go, float opacity) {
        while (go.GetComponent<Renderer>().material.GetFloat("_Opacity") != opacity) {
            print(go.GetComponent<Renderer>().material.GetFloat("_Opacity") + " = " + opacity);
            //float a = go.GetComponent<Renderer>().material.GetFloat("_Opacity");
            go.GetComponent<Renderer>().material.SetFloat("_Opacity", Mathf.Lerp(actualOpacity, opacity, lerpTime));
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
