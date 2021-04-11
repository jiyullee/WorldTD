using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Europe : Synergy
{
    public override void ActiveSynergy()
    {
        if (TargetTower != null)
        {
            TargetTower.IncreaseSpeed(cur_changeAmount);
        }
    }
}
