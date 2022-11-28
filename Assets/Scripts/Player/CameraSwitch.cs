using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject CameraObject;

    private GameManager gameManager;
    private Quaternion rotation;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rotation = CameraObject.transform.rotation;
    }

    private void Update()
    {
        rotation *= Quaternion.AngleAxis(gameManager.GetPlayerController().playerInput.Player.Camera.ReadValue<Vector2>().x * Time.deltaTime * 10, Vector3.up);
        rotation *= Quaternion.AngleAxis(gameManager.GetPlayerController().playerInput.Player.Camera.ReadValue<Vector2>().y * Time.deltaTime * 10, Vector3.right);


        Vector3 bounds = new Vector3(30, 180, 1);
        rotation = ClampRotation(rotation, bounds);
        CameraObject.transform.rotation = Quaternion.Slerp(rotation, transform.rotation, Time.deltaTime);
    }

    public static Quaternion ClampRotation(Quaternion q, Vector3 bounds)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, 10, 30);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

        return q;
    }
}
