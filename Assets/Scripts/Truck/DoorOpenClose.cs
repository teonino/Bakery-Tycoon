using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose : MonoBehaviour
{
    [SerializeField] GameObject truck;
    private Vector3 openPosition = new Vector3(-7.2f, 0, 5f);
    private Vector3 closePosition = new Vector3(-7.2f, 0, 2.5f);
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (Vector3.Distance(transform.position, truck.transform.position) < 3f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, openPosition, ref velocity, 0.8f);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, closePosition, ref velocity, 2f);
        }
    }
}
