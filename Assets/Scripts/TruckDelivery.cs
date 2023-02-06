using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDelivery : MonoBehaviour
{
    [SerializeField] private GameObject pathPoint1;
    [SerializeField] private GameObject pathPoint2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ListDeliveries deliveries;
    [SerializeField] private TruckDeliveryTime time;
    private bool moving;
    private bool isOpen;
    private Vector3 dest;
    private Vector3 velocity = Vector3.zero;
    private Delivery delivery;

    private void Start()
    {
        //this.gameObject.SetActive(false);
        DeliveryDeparture();
    }

    private void FixedUpdate()
    {
        if (moving)
            transform.position = Vector3.SmoothDamp(transform.position, dest, ref velocity, time.GetTime());

        if (Vector3.Distance(transform.position, dest) < 0.1f && moving)
        {
            moving = false;
            audioSource.Stop();
            if (delivery != null)
            {
                deliveries.DeliverOrder(delivery);
            }
        }
    }

    //Trigger this when truck is close to arriving
    public void DeliveryArriving()
    {
        if (!moving)
        {
            delivery = deliveries.GetDeliveries();
            gameObject.transform.position = new Vector3(pathPoint2.transform.position.x, pathPoint2.transform.position.y, pathPoint2.transform.position.z);
            gameObject.SetActive(true);
            moving = true;
            dest = pathPoint1.transform.position;
            audioSource.Play();
        }
    }

    //Trigger this when the player interacted with the truck to collect the delivery
    private void DeliveryDeparture()
    {
        gameObject.transform.position = new Vector3(pathPoint1.transform.position.x, pathPoint1.transform.position.y, pathPoint1.transform.position.z);
        gameObject.SetActive(true);
        dest = pathPoint2.transform.position;
        moving = true;
        audioSource.Play();
    }
}
