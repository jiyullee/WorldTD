using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asia : Synergy
{
    public override void ActiveSynergy()
    {
        if(TargetTower != null)
            TargetTower.DecreaseMonsterSpeed(cur_changeAmount);
    }
}
