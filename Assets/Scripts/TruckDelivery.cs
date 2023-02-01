using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDelivery : MonoBehaviour
{
    [SerializeField] private GameObject PathPoint1;
    [SerializeField] private GameObject PathPoint2;
    

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    //Trigger this when truck is close to arriving
    void DeliveryArriving()
    {
        this.gameObject.transform.position = new Vector3(PathPoint2.transform.position.x, PathPoint2.transform.position.y, PathPoint2.transform.position.z);
        this.gameObject.SetActive(true);
        Vector3.Lerp(this.transform.position, PathPoint1.transform.position, 5f);
    }

    //Trigger this when the player interacted with the truck to collect the delivery
    void DeliveryDeparture()
    {
        this.gameObject.transform.position = new Vector3(PathPoint1.transform.position.x, PathPoint1.transform.position.y, PathPoint1.transform.position.z);
        this.gameObject.SetActive(true);
        Vector3.Lerp(this.transform.position, PathPoint2.transform.position, 5f);
    }
}
