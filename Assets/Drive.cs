using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    [SerializeField] private float carSpeed = 10f;
    [SerializeField] private GameObject PathPointVertical;
    [SerializeField] private GameObject PathPointHorizontal;
    [SerializeField] private OutdoorManager vehiculeSpawner;
    [SerializeField] private List<Transform> PathPointVerticalChildren = new List<Transform>();
    [SerializeField] private List<Transform> PathPointHorizontalChildren = new List<Transform>();
    private bool canDrive;
    private bool horizontalPathTaken;
    private bool verticalPathTaken;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        vehiculeSpawner = gameObject.GetComponentInParent<OutdoorManager>();
        PathPointVertical = vehiculeSpawner.HorizontalPathPointParent;
        PathPointHorizontal = vehiculeSpawner.VerticalPathPointParent;

        if (Vector3.Distance(gameObject.transform.position, vehiculeSpawner.spawnPoint[0].transform.position) < 1)
        {
            horizontalPathTaken = true;
            verticalPathTaken = false;
        }
        else if (Vector3.Distance(gameObject.transform.position, vehiculeSpawner.spawnPoint[1].transform.position) < 0.1)
        {
            horizontalPathTaken = false;
            verticalPathTaken = true;
        }

        for (int i = 0; i < PathPointVertical.transform.childCount; i++)
        {
            PathPointVerticalChildren.Add(PathPointVertical.transform.GetChild(i));

        }
        for (int j = 0; j < PathPointHorizontal.transform.childCount; j++)
        {
            PathPointHorizontalChildren.Add(PathPointHorizontal.transform.GetChild(j));
        }
        //DriveStart();
        print("car initialized");
        canDrive = true;
    }

    private int index = 0;

    private void FixedUpdate()
    {
        if (canDrive)
        {
            if (Vector3.Distance(gameObject.transform.position, PathPointVerticalChildren[index].position) < 0.1f)
            {
                index++;
                //gameObject.transform.LookAt(PathPointVerticalChildren[index]);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, PathPointVerticalChildren[index].rotation, 0.1f);
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, PathPointVerticalChildren[index].transform.position, ref velocity, 1f, carSpeed);
            }
            if ( index == PathPointVerticalChildren.Count)
            {
                vehiculeSpawner.destroyLastVehicule();
            }
        }
        //else if (horizontalPathTaken)
        //{
        //    gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, PathPointVerticalChildren[1].transform.position, ref velocity, 1, carSpeed);
        //}

    }
}
