using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private List<Material> materials;
    private Camera cam;
    Vector3 pos = new Vector3(Screen.width/2, Screen.height/2, 0);
    [SerializeField] private GameObject wallPanel;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

        Ray ray = cam.ScreenPointToRay(pos);
        Debug.DrawRay(ray.origin, ray.direction * 15, Color.yellow);
        if (Physics.Raycast(ray, out var hitInfo, 15))
        {
            print("Raycast detecte des choses");
           if(hitInfo.collider.gameObject.name.Contains("Wall"))
           {
                print("Raycast détecte des walls");
                int children = transform.childCount;
              for (int i = 0; i < children; ++i)
              {
                    print("entrer dans le for");
                    print("For loop: " + transform.GetChild(i));
              }
            }
        }
    }

}
