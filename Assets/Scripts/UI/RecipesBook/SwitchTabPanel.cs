using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwitchTabPanel : MonoBehaviour {
    [Header("Tab Management")]
    [SerializeField] private List<GameObject> TabImage;
    [SerializeField] private int TabIndex = 0;

    [Header("Image Used for the selected and unselected Tabs")]
    [SerializeField] private List<Sprite> TabsButton;

    public void GoOnNextTab() {
        if (TabIndex + 1 > TabImage.Count - 1) {
            TabIndex = TabImage.Count - 1;
        }
        else {
            TabIndex++;
            for (int i = 0; i < TabImage.Count; i++) {
                TabImage[i].GetComponent<Image>().sprite = TabsButton[0];
            }

            TabImage[TabIndex].GetComponent<Image>().sprite = TabsButton[1];
        }
    }

    public void GoOnPreviousTab() {
        if (TabIndex - 1 < 0) {
            TabIndex = 0;
        }
        else {
            TabIndex--;
            for (int i = 0; i < TabImage.Count; i++) {
                TabImage[i].GetComponent<Image>().sprite = TabsButton[0];
            }
            TabImage[TabIndex].GetComponent<Image>().sprite = TabsButton[1];
        }
    }
}
