using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oceania : Synergy
{
    public override void ActiveSynergy()
    {
        TowerManager.Instance.IncreaseAttack(cur_changeAmount);
    }
}
