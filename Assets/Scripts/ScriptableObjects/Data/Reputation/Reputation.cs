using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reputation", menuName = "Data/Reputation")]
public class Reputation : ScriptableObject {
    [SerializeField] private int level;
    [SerializeField] private int experience;
    [SerializeField] private List<int> experiencesPerLevel;
    [SerializeField] private Action UpdateUI;

    private void OnEnable() {
        experience = 0;
        level = 0;
    }

    public int GetLevel() => level;
    public int GetExperience() => experience;

    public void SetUpdateUI(Action updateUI) => UpdateUI = updateUI;

    public int GetExpNeeded() => experiencesPerLevel[level];

    public void AddReputation(int value) {
        experience += value;

        if (experience > experiencesPerLevel[level]) {
            level++;
            experience = 0;
        }
        UpdateUI?.Invoke();
    }
    public void RemoveReputation(int value) {
        experience -= value;
        if (experience < 0)
            experience = 0;
        UpdateUI?.Invoke();
    }
}
