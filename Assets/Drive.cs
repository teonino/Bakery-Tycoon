using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    [SerializeField] private float carSpeed = 10f;
    [SerializeField] private MeshRenderer _vehicleMesh;

    public MeshRenderer vehicleMesh => _vehicleMesh;

    private List<Transform> _pathPoints = new List<Transform>();

    private bool canDrive;
    internal bool isCar;
    internal int indexVehicule;
    private Vector3 velocity = Vector3.zero;


    private int index = 0;

    private void FixedUpdate()
    {
        if (canDrive)
        {
            if (_pathPoints.Count > 0)
            {
                if (index == _pathPoints.Count)
                {
                    Destroy(gameObject);
                }
                if (gameObject != null)
                {
                    try
                    {
                        if (Vector3.Distance(gameObject.transform.position, _pathPoints[index].position) < 0.1f)
                        {
                            index++;
                            //gameObject.transform.LookAt(PathPointVerticalChildren[index]);
                        }
                        else
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, _pathPoints[index].rotation, 0.1f);
                            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, _pathPoints[index].transform.position, ref velocity, 1f, carSpeed);
                        }
                    }
                    catch
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

    }

    public void SetPath(List<Transform> path)
    {
        _pathPoints = path;
        if (_pathPoints.Count > 0)
        {
            this.transform.position = _pathPoints[0].transform.position;
            canDrive = true;
        }
    }

}
