using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asia : Synergy
{
    public override void ActiveSynergy()
    {
        TargetTower.DecreaseMonsterSpeed(cur_changeAmount);
    }
}
