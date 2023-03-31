using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private Animation cashRegisterPay;

    private void Start()
    {
        cashRegisterPay = GetComponent<Animation>();
    }

    public void Payment()
    {
        cashRegisterPay.Play();
    }
}
