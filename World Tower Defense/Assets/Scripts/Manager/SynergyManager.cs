using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SynergyManager : UnitySingleton<SynergyManager>
{
    public int synergy_count { get; private set; }
    //시너지별, 타워 종류별 분류된 테이블
    public Dictionary<string, Dictionary<string, bool>> SynergyTable = new Dictionary<string, Dictionary<string, bool>>();
    
    //시너지별 활성 카운트
    public Dictionary<string, List<int>> SynergyCount = new Dictionary<string, List<int>>();
    public Dictionary<string, int> SynergyIndex = new Dictionary<string, int>();
    public override void OnCreated()
    {
        synergy_count = SynergyData.Instance.GetTable().Count;
        for (int i = 0; i < synergy_count; i++)
        {
            string synergyName = SynergyData.Instance.GetTableData(i).SynergyName;
            List<string> list_towerName = SynergyData.Instance.GetTableData(i).TowerList;

            Dictionary<string, bool> dic_tower = new Dictionary<string, bool>();
            for (int j = 0; j < list_towerName.Count; j++)
            {
                dic_tower.Add(list_towerName[j], false);
            }

            SynergyTable.Add(synergyName, dic_tower);
            SynergyCount.Add(synergyName, new List<int>());
            SynergyCount[synergyName] = SynergyData.Instance.GetTableData(i).ActivateNum;
            SynergyIndex.Add(synergyName, 0);
        }
        
    }

    public override void OnInitiate()
    {
        
    }

    private void SetSynergyUI()
    {
        for (int i = 0; i < synergy_count; i++)
        {
            string synergyName = SynergyData.Instance.GetTableData(i).SynergyName;
            int idx = SynergyIndex[synergyName];
            SynergyUI.Instance.SetSynergyUIs(i, idx);    
        }
        
    }
    public void SetSynergy()
    {
        CheckSynergy();
        CheckCount();
        ActiveSynergy();
        SetSynergyUI();
    }

    private void CheckSynergy()
    {
        List<Tower> list_tower = TowerManager.Instance.list_tower;
        
        for (int i = 0; i < list_tower.Count; i++)
        {
            string towerName = list_tower[i].TowerName;
            string[] synergyNames = list_tower[i].SynergyNames;

            for (int j = 0; j < synergyNames.Length; j++)
            {
                string synergyName = synergyNames[j];
                if(SynergyTable[synergyName].ContainsKey(towerName))
                    SynergyTable[synergyName][towerName] = true;
            }
        }
        
    }

    private void CheckCount()
    {
        for (int i = 0; i < synergy_count; i++)
        {
            string synergyName = SynergyData.Instance.GetTableData(i).SynergyName;
            List<string> list_towerName = SynergyData.Instance.GetTableData(i).TowerList;

            int count = 0;
            for (int j = 0; j < list_towerName.Count; j++)
            {
                if (SynergyTable[synergyName][list_towerName[j]])
                {
                    count++;
                }
            }

            int index = 0;
            List<int> list_activateCount = SynergyCount[synergyName];

            if (count == list_activateCount[list_activateCount.Count - 1])
                index = list_activateCount.Count - 1;
            else
            {
                for (int j = 0; j < list_activateCount.Count - 1; j++)
                {
                    if (list_activateCount[j] <= count && count < list_activateCount[j + 1])
                    {
                        index = j;
                        break;
                    }
                }    
            }

            SynergyIndex[synergyName] = index;

        }
    }

    public int GetActivateIndex(string p_synergyName)
    {
        return SynergyIndex[p_synergyName];
    }

    private void ActiveSynergy()
    {
        List<Tower> list_tower = TowerManager.Instance.list_tower;
        for (int i = 0; i < list_tower.Count; i++)
        {
            if(list_tower[i] != null)
                list_tower[i].ActiveSynergy();
        }
    }
}
