using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{

    [SerializeField] private Transform parentObject;
    [SerializeField] private float hideSpeed = 2f;
    [SerializeField] private float showSpeed = 2f;
    //[SerializeField] private float lerpTime = 0.5f;

    private bool isHiding = false;
    private bool isShowing = false;
    private float hideOpacity = 1f;
    private float showOpacity = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHiding = true;
            isShowing = false;
            hideOpacity = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHiding = false;
            isShowing = true;
            showOpacity = 0;
        }
    }

    private void Update()
    {
        if (isHiding)
        {
            hideOpacity = Mathf.Lerp(hideOpacity, 0f, hideSpeed * Time.deltaTime);
            SetOpacity(hideOpacity);
        }
        else if (isShowing)
        {
            showOpacity = Mathf.Lerp(showOpacity, 1f, showSpeed * Time.deltaTime);
            SetOpacity(showOpacity);
        }

    }

    private void SetOpacity(float amount)
    {
        foreach (Transform child in parentObject)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            float actualOpacity = renderer.material.GetFloat("_Opacity");
            renderer.material.SetFloat("_Opacity", amount);
        }
    }
}