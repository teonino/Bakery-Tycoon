using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject CameraObject;
    [SerializeField] private float rotateSpeed = 300f;
    public GameManager gameManager;

    private void Update()
    {
        Quaternion rotateDir = new Quaternion();
        rotateDir *= Quaternion.AngleAxis(gameManager.GetPlayerController().playerInput.Player.Camera.ReadValue<Vector2>().x * Time.deltaTime * 10, Vector3.up);


        //if (Input.GetKey(KeyCode.D))
        //{
        //    rotateDir -= 1f;
        //}

        transform.rotation = rotateDir;

    }

}
