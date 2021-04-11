using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Africa : Synergy
{
    public override void ActiveSynergy()
    {
        if(TargetTower != null)
            TargetTower.DamageBuff(cur_changeAmount);
    }
}
