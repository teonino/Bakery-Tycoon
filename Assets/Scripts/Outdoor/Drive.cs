using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drive : MonoBehaviour
{
    
    public enum Lane
    {
        Horizontal,
        Vertical
    }
    
    
    [SerializeField] private float carSpeed = 10f;
    [SerializeField] private MeshRenderer _vehicleMesh;
    [SerializeField] private Vector2 _collideSize;
    [SerializeField] private float _collideDistance = 4;

    public MeshRenderer vehicleMesh => _vehicleMesh;

    private List<Transform> _pathPoints = new List<Transform>();
    private Lane _laneUsed;
    public Lane LaneUsed
    {
        get { return _laneUsed; }
        set { _laneUsed = value; }
    }

    private bool canDrive;
    internal bool isCar;
    internal int indexVehicule;
    private Vector3 velocity = Vector3.zero;

    private int index = 0;

    private void Awake()
    {
        canDrive = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (transform.forward * _collideDistance), new Vector3(_collideSize.x, 2, _collideSize.y));
    }

    private void Update()
    {

        if (canDrive == true)
        { 
            List<Collider> colliderToCheck = Physics.OverlapBox(transform.position + (transform.forward * _collideDistance), new Vector3(_collideSize.x, 2, _collideSize.y)*0.5f).ToList();
            foreach (Collider collider in colliderToCheck)
            {
                if (collider.TryGetComponent<Drive>(out var vehicle))
                {
                    if (collider.gameObject != this.gameObject && vehicle.LaneUsed == _laneUsed)
                    {
                        StartCoroutine(waitForRestart());
                    }
                }
                else if (collider.TryGetComponent<AICustomer>(out var customer) || collider.TryGetComponent<AIRegularCustomer>(out var regularCustomer))
                {
                    StartCoroutine(waitForRestart());
                }
            }
            
        }


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
            this.transform.rotation = _pathPoints[0].transform.rotation;
        }
    }

    IEnumerator waitForRestart()
    {
        canDrive = false;
        yield return new WaitForSeconds(1.2f);
        canDrive = true;
    }

}
