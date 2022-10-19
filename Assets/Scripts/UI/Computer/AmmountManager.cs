using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmountManager : MonoBehaviour
{
    [SerializeField] private int AmmountToBuy;
    private bool MinusButton;
    private bool PlusButton;
    [SerializeField] private TextMeshProUGUI TextAmmount;
    [SerializeField] private GameObject ButtonPanel;

    public void IncreaseDecreaseAmmount()
    {
        if (MinusButton)
        {
            if (AmmountToBuy <= 0)
            {
                Debug.LogWarning("variable set to 0");
                AmmountToBuy = 0;
            }
            else
            {
                print("MinusButtonClicked");
                AmmountToBuy -= 1;
                TextAmmount.text = AmmountToBuy.ToString();
            }
        }
        else if (PlusButton)
        {
            print("PlusButtonClicked");
            AmmountToBuy += 1;
                TextAmmount.text = AmmountToBuy.ToString();
        }
    }

    public void MinusButtonIsClicked()
    {
        MinusButton = true;
        PlusButton = false;
        IncreaseDecreaseAmmount();
    }

    public void PlusButtonIsClicked()
    {
        MinusButton = false;
        PlusButton = true;
        IncreaseDecreaseAmmount();
    }

}
