using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDelivery : Interactable {
    [SerializeField] private GameObject pathPoint1;
    [SerializeField] private GameObject pathPoint2;
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private ListDeliveries deliveries;
    [SerializeField] private TruckDeliveryTime time;
    [SerializeField] private NotificationEvent notifEvent;
    [SerializeField] private NotificationType notifType;
    [SerializeField] private SFXPlayer sfxPlayer;
    [SerializeField] private ParticleSystem vfx;

    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip SFXClip;

    private bool moving;
    private bool fetchingOrder = false;
    private Vector3 dest;
    private Vector3 velocity = Vector3.zero;
    private Delivery delivery;

    private void Awake()
    {
        vfx.Stop();
    }

    private void FixedUpdate() {
        if (moving)
            transform.position = Vector3.SmoothDamp(transform.position, dest, ref velocity, time.GetTime());

        if (pathPoint1 && pathPoint2) {
            if (Vector3.Distance(transform.position, pathPoint2.transform.position) < 0.1f && moving && fetchingOrder) {
                DeliveryArriving();
            }
            if (Vector3.Distance(transform.position, pathPoint1.transform.position) < 0.1 && moving && !fetchingOrder) {
                if (delivery != null)
                    notifEvent.Invoke(notifType);

                moving = false;
                vfx.Stop();
                SFXSource.PlayOneShot(SFXClip);
                engineSound.Stop();
            }
        }

        if (delivery != null && moving == false) {
            gameObject.layer = LayerMask.NameToLayer("Outline");
        }
        else {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void DeliveryDeparture() {
        if (!moving && delivery == null) {
            gameObject.transform.position = new Vector3(pathPoint1.transform.position.x, pathPoint1.transform.position.y, pathPoint1.transform.position.z);
            gameObject.SetActive(true);
            dest = pathPoint2.transform.position;
            moving = true;
            fetchingOrder = true;
            vfx.Play();
            engineSound.Play();
        }
    }

    public bool CanAddDelivery() => !moving && delivery == null;

    //Trigger this when truck is close to arriving
    public void DeliveryArriving() {
        delivery = deliveries.GetDeliveries();
        gameObject.transform.position = new Vector3(pathPoint2.transform.position.x, pathPoint2.transform.position.y, pathPoint2.transform.position.z);
        gameObject.SetActive(true);
        moving = true;
        fetchingOrder = false;
        dest = pathPoint1.transform.position;
        engineSound.Play();
    }

    public override void Effect() {
        if (delivery != null && !moving) {
            deliveries.DeliverOrder(delivery);
            sfxPlayer.InteractSound();
            delivery = null;
        }
    }

    public override bool CanInterract() {
        canInterract = delivery != null && !moving;
        return canInterract;
    }
}
