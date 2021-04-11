using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouthAmerica : Synergy
{
    public override void ActiveSynergy()
    {
        if (TargetTower != null)
        {
            TargetTower.DecreaseArmor(cur_changeAmount);
        }
    }
}
