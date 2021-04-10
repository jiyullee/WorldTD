using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

abstract public class Synergy : MonoBehaviour
{
    protected SYNERGY SynergyType;
    protected int cur_idx = 0;
    protected float cur_changeAmount = 0;
    protected float[] changeAmount = new float[4];
    protected Tower TargetTower;
    public abstract void ActiveSynergy();

    private void Awake()
    {
        
    }

    public void SetSynergy(Tower p_tower, string p_synergyName, int idx)
    {
        
        SynergyType = (SYNERGY)System.Enum.Parse(typeof(SYNERGY), p_synergyName);
        TargetTower = p_tower;
        
        int key = GetKey();
        changeAmount = SynergyData.Instance.GetTableData(key).Increase.ToArray();
        
        if (cur_idx != idx)
        {
            cur_idx = idx;
            cur_changeAmount = changeAmount[cur_idx];
            ActiveSynergy();
        }
    }
    
    private int GetKey()
    {
        List<string> synergyNames = System.Enum.GetNames(typeof(SYNERGY)).ToList();
        return synergyNames.IndexOf(SynergyType.ToString());
    }
}
