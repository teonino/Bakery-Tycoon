using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{

    [SerializeField] private GameObject wallToDispawn;
    [SerializeField][Range(5f, 15f)] float time;
    private float lerpTime;
    public bool thisRoomIsActive;
    private List<Coroutine> invisibleCoroutines;
    private List<Coroutine> visibleCoroutines;

    private void Start()
    {
        lerpTime = time * Time.deltaTime;
        invisibleCoroutines = new List<Coroutine>();
        visibleCoroutines = new List<Coroutine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (thisRoomIsActive)
        {
            if (other.gameObject.name == "Main Camera")
            {
                if (visibleCoroutines.Count > 0) {
                    foreach (Coroutine coroutine in visibleCoroutines)
                        StopCoroutine(coroutine);
                    visibleCoroutines.Clear();
                }

                for (int i = 0; i < wallToDispawn.transform.childCount; i++)
                {
                    invisibleCoroutines.Add(StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), 0)));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (thisRoomIsActive)
        {
            if (other.gameObject.name == "Main Camera")
            {
                if (invisibleCoroutines.Count > 0) {
                    foreach (Coroutine coroutine in invisibleCoroutines)
                        StopCoroutine(coroutine);
                    invisibleCoroutines.Clear();
                }

                for (int i = 0; i < wallToDispawn.transform.childCount; i++)
                {
                    visibleCoroutines.Add(StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), 1)));
                }
            }
        }
    }

    private IEnumerator ChangeColor(Transform go, float opacity)
    {
        while (go.GetComponent<Renderer>().material.color.a != opacity)
        {
            Color c = go.GetComponent<Renderer>().material.color;
            go.GetComponent<Renderer>().material.color = Color.Lerp(c, new Color(c.r,c.g,c.b, opacity), lerpTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
