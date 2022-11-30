using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{

    [SerializeField] private GameObject wallToDispawn;
    private List<Color> colors;
    [SerializeField][Range(5f, 15f)] float time;
    private float lerpTime;
    Color fade = new Color(0, 0, 0, 0);

    private void Start()
    {
        colors = new List<Color>();
        lerpTime = time * Time.deltaTime;
        foreach (Transform t in wallToDispawn.transform)
            colors.Add(t.GetComponent<Renderer>().material.color);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Main Camera")
        {
            for (int i = 0; i < wallToDispawn.transform.childCount; i++)
            {
                StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), fade));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main Camera")
        {
            for (int i = 0; i < wallToDispawn.transform.childCount; i ++)
            {
                StartCoroutine(ChangeColor(wallToDispawn.transform.GetChild(i), colors[i]));
            }
        }
    }

    private IEnumerator ChangeColor(Transform go, Color finalColor)
    {
        while (go.GetComponent<Renderer>().material.color != finalColor)
        {

            Color c = go.GetComponent<Renderer>().material.color;
            go.GetComponent<Renderer>().material.color = Color.Lerp(c, finalColor, lerpTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
