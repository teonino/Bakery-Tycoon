using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularHolder : MonoBehaviour
{
    [SerializeField] private List<RegularSO> regulars;

    public RegularSO GetRegular(RegularSO regular)
    {
        foreach(RegularSO s in regulars)
        {
            if(regular.name == s.name)
            {
                return s;
            }
        }
        return null;
    }
}
