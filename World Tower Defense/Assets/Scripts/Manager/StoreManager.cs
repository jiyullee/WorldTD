using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StoreManager : UnitySingleton<StoreManager>
{
    public List<TowerInstance>[] list_all_towers;
    private List<TowerInstance> list_remain_towers;
    private int max_country;
    private int[] weight = {30, 20, 20, 20, 10};
    private int max_store = 5;
    public override void OnCreated()
    {
        list_all_towers = new List<TowerInstance>[5 + 1];
        for (int i = 1; i < 5 + 1; i++)
        {
            list_all_towers[i] = new List<TowerInstance>();
        }
        AddList();
        RefreshStore();
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

    /// <summary>
    /// 상점에서 코스트 랜덤 선택
    /// </summary>
    /// <returns></returns>
    private int SelectRand()
    {
        float rand = Random.Range(0, 100);
        int cost = 0;
        float sum = weight[cost];
        while (true)
        {
            if (rand <= sum)
            {
                return cost + 1;
            }
            sum += weight[++cost];
        }
    }
    public void RefreshStore()
    {
        for (int i = 0; i < max_store; i++)
        {
            int cost = SelectRand();
            StoreUI.Instance.Refresh(i, list_all_towers[cost][Random.Range(0, list_all_towers[cost].Count)]);
        }
    }

    public void BuyTower()
    {
        
    }

    private void CreateTower()
    {
        
    }

}