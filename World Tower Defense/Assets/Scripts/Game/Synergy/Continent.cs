using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent : Synergy
{
    public override void ActiveSynergy()
    {
        if (TargetTower != null)
        {
            TargetTower.IncreaseTrueDamage(cur_changeAmount);
        }
    }
}
