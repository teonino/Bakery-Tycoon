using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reputation
{
    public int level;
    public int experience;

    public Reputation() {
        level = 0;
        experience = 0;
    }

    public Reputation(int level, int experience) {
        this.level = level;
        this.experience = experience;
    }
}
