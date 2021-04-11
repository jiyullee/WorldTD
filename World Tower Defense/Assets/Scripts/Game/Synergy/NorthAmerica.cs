using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthAmerica : Synergy
{
    public override void ActiveSynergy()
    {
        TowerManager.Instance.IncreaseRange(cur_changeAmount);
    }
}
