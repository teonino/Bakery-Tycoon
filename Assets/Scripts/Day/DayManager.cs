using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

    private GameManager gameManager;
    
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private int morningTime;
    [SerializeField] private int dayTime;
    [SerializeField] private float timeElapsed;
    private int duration;

    // Start is called before the first frame update
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        duration = morningTime + dayTime;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (timeElapsed < duration) {
            transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / duration));
            timeElapsed += Time.deltaTime;

            if (timeElapsed > morningTime && gameManager.GetDayTime() == DayTime.Morning)
                gameManager.SetDayTime();
        }
        else if (gameManager.GetDayTime() == DayTime.Day)
            gameManager.SetDayTime();
    }
}
