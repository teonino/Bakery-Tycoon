using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CommandListManager : MonoBehaviour {

    [SerializeField] private AssetReference commandRecap;
    [SerializeField] private CustomerWaitingTime waitingTimeSO;

    private List<CommandRecap> commands = new List<CommandRecap>();

    public void AddCommand(AICustomer customer) {
        commandRecap.InstantiateAsync(gameObject.transform).Completed += (go) => {
            CommandRecap recap = go.Result.GetComponent<CommandRecap>();
            recap.SetCustomer(customer);
            recap.SetWaitingTime(waitingTimeSO);
            recap.StartCoroutineText();
            commands.Add(recap);
        };
        if (customer.isRegular())
            print("Regular");
        else
            print("Random");
    }

    public void RemoveCommand(AICustomer customer) {
        CommandRecap recap = null;

        for (int i = 0; i < commands.Count && !recap; i++) 
            if (commands[i].GetCustomer() == customer) 
                recap = commands[i];

        commands.Remove(recap);
        Addressables.ReleaseInstance(recap.gameObject);
    }
}
