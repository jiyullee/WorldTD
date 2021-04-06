using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StoreManager : UnitySingleton<StoreManager>
{
    public List<TowerInstance>[] list_all_towers;
    private List<TowerInstance> list_remain_towers;
    private int max_country;
    private int[] weight = {100, 0, 0, 0, 0};
    public override void OnCreated()
    {
        list_all_towers = new List<TowerInstance>[5 + 1];
        for (int i = 1; i < 5 + 1; i++)
        {
            list_all_towers[i] = new List<TowerInstance>();
        }
        AddList();
    }

    public override void OnInitiate()
    {
        
    }

    private void AddList()
    {
        max_country = StoreTowerData.Instance.GetTable().Count;
        for (int i = 0; i < max_country; i++)
        {
            int towerIndex = StoreTowerData.Instance.GetTableData(i).TowerIndex;
            int count = StoreTowerData.Instance.GetTableData(i).Count;
            int cost = TowerData.Instance.GetTableData(towerIndex).Cost;
            TowerInstance towerInstance = new TowerInstance(towerIndex);
            list_all_towers[cost].Add(towerInstance);
        }
    }

    public void RefreshStore()
    {
        
    }

}