using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwitchTabPanel : MonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] private PlayerControllerSO controller;

    [Header("Tab Management")]
    [SerializeField] private List<GameObject> TabImage;
    [SerializeField] private int TabIndex = 0;

    [Header("Image Used for the selected and unselected Tabs")]
    [SerializeField] private List<Sprite> TabsButton;

    void Start()
    {
        controller.GetPlayerController().playerInput.Tabs.NextTab.Enable();
        controller.GetPlayerController().playerInput.Tabs.PreviousTab.Enable();
        controller.GetPlayerController().playerInput.Tabs.NextTab.performed += GoOnNextTab;
        controller.GetPlayerController().playerInput.Tabs.PreviousTab.performed += GoOnPreviousTab;
    }

    public void GoOnNextTab(InputAction.CallbackContext context)
    {

        if(TabIndex+1 > TabImage.Count-1)
        {
            TabIndex = TabImage.Count-1;
        }
        else
        {
            TabIndex++;
            for (int i = 0; i < TabImage.Count; i++)
            {
                TabImage[i].GetComponent<Image>().sprite = TabsButton[0];
            }

            TabImage[TabIndex].GetComponent<Image>().sprite = TabsButton[1];
        }

    }

    public void GoOnPreviousTab(InputAction.CallbackContext context)
    {

        if (TabIndex-1 < 0)
        {
            TabIndex = 0;
        }
        else
        {
            TabIndex--;
            for (int i = 0; i < TabImage.Count; i++)
            {
                TabImage[i].GetComponent<Image>().sprite = TabsButton[0];
            }
            TabImage[TabIndex].GetComponent<Image>().sprite = TabsButton[1];
        }
    }

}
