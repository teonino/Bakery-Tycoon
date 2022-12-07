using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameratest;
    [SerializeField] private Vector3 playerposition;
    [SerializeField] private Vector3 cameraPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cameratest.transform.position = new Vector3(player.transform.position.x + 2, cameratest.transform.position.y , player.transform.position.z - 2);
    }
}
