using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    private List<Color> colors;
    private GameObject wallHit;

    private Camera cam;
    Vector3 pos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    [SerializeField] private GameObject wallPanel;
    public float[] opacityValue;

    private void Start()
    {
        cam = Camera.main;
        colors = new List<Color>();
    }

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(pos), out hit, 5);

        if (hit.collider && hit.collider.gameObject.name.Contains("Wall"))
        {
            if (hit.collider.gameObject != wallHit)
            {
                {
                    wallHit = hit.collider.gameObject;
                    foreach (Transform t in hit.transform)
                    {
                        Color c = t.gameObject.GetComponent<Renderer>().material.color;
                        colors.Add(new Color(c.r, c.g, c.b, c.a));
                        t.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
                    }
                }
            }
        }
        else
        {
            if (wallHit)
            {
                for (int i = 0; i < wallHit.transform.childCount; i++)
                {
                    wallHit.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = colors[i];
                }
                wallHit = null;
            }
        }
    }
}
