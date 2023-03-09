using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceDoor : Interactable {
    [SerializeField] private Day day;
    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;
    [SerializeField] private InterractQuest finishDayQuest;
    private bool isClosing = false;

    public override void Effect() {
        if (day.GetDayTime() == DayTime.Evening) {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Door");

            if (gos.Length > 1) {
                door1 = gos[0];
                door2 = gos[1];
                SaveManager save = FindObjectOfType<SaveManager>();
                save?.Save();
                StartCoroutine(ClosingDoors());
            }
        }

        finishDayQuest?.OnInterract();
    }

    public override bool CanInterract() {
        canInterract = day.GetDayTime() == DayTime.Evening;
        return canInterract;
    }

    private void FixedUpdate() {
        if (isClosing) {
            door1.transform.rotation = Quaternion.Lerp(door1.transform.rotation, Quaternion.Euler(0, -180, 0), 0.1f);
            door2.transform.rotation = Quaternion.Lerp(door2.transform.rotation, Quaternion.Euler(0, 0, 0), 0.1f);
        }
    }

    private IEnumerator ClosingDoors() {
        isClosing = true;
        yield return new WaitForSeconds(2f);
        day.OnNewDay();
        SceneManager.LoadScene("FirstBakery_New");
    }
}
