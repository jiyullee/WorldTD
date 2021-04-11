using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peninsula : Synergy
{
    public override void ActiveSynergy()
    {
        if(TargetTower != null)
            TargetTower.AroundAttack(cur_changeAmount);
    }
}
