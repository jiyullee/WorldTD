using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StoreManager : UnitySingleton<StoreManager>
{
    public List<TowerInstance>[] list_all_towers;
    private List<TowerInstance> list_remain_towers;
    private int max_country;
    private int gold;
    public int interest => gold / 10;
    private int level;
    private int max_level;
    private int exp;
    private int max_exp;
    private float[] rarity = {0};
    private int max_store; // 상점 구매 칸 수
    public static TowerInstance selectedTowerInstance;
    public static bool isSelecting;
    
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
        InitState();
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
        float rand = Random.Range(0f, 1f);
        int cost = 0;
        float sum = rarity[cost];
        while (true)
        {
            if (rand <= sum)
            {
                return cost + 1;
            }
            sum += rarity[++cost];
        }
    }
    public void RefreshStore()
    {
        if (gold < 1)
            return;

        gold -= 1;
        SetGoldUI();
        
        for (int i = 0; i < max_store; i++)
        {
            int cost = SelectRand();
            StoreUI.Instance.Refresh(i, list_all_towers[cost][Random.Range(0, list_all_towers[cost].Count)]);
        }
    }
    
    public void SetSelectedInstance(TowerInstance p_towerInstance)
    {
        selectedTowerInstance = p_towerInstance;
        isSelecting = true;
    }
    
    public void BuyTower()
    {
        if (selectedTowerInstance == null) return;

        int cost = selectedTowerInstance.GetTowerData().Cost;
        
        if (gold < cost)
            return;
        
        gold -= cost;
        SetGoldUI();
    }

    public void EarnGold()
    {
        gold += 6;
        gold += interest;
        SetGoldUI();
    }

    public void SetNullInstance()
    {
        selectedTowerInstance = null;
        isSelecting = false;
    }

    public Tower CreateTower(Vector3 p_pos)
    {
        //타워 생성 후 타워 정보 삭제 -> 배치 가능한 버튼 OFF & 기물 구매 가능 ON
        Tower tower = TowerManager.Instance.CreateTower(selectedTowerInstance, p_pos);
        SetNullInstance();
        MapUI.Instance.SetViewSelectableButtons(false);
        StoreUI.Instance.SetActiveButtons(true);
        return tower;
    }

    private void InitState()
    {
        max_store = 5;
        level = 1;
        max_level = 8;
        rarity = StoreData.Instance.GetTableData(level).Rarity.ToArray();
        exp = 0;
        max_exp = StoreData.Instance.GetTableData(level).MaxExp;
        EarnGold();
        SetLevelUI();
        SetExpUI();
    }

    public void ExpUp(int increase)
    {
        //골드 부족
        if (gold < 5)
            return;

        gold -= 5;
        
        if (exp + increase < max_exp)
            exp += increase;
        else if(exp + increase >= max_exp)
        {
            exp = exp + increase - max_exp;
            LevelUp();
        }

        //경험치, 골드 UI 적용
        SetExpUI();
        SetGoldUI();
        
    }
    
    public void LevelUp()
    {
        level = level + 1 <= max_level ? level + 1 : max_level;
        rarity = StoreData.Instance.GetTableData(level).Rarity.ToArray();
        max_exp = StoreData.Instance.GetTableData(level).MaxExp;
        
        //레벨 UI 적용
        SetLevelUI();
    }

    private void SetExpUI()
    {
        StoreUI.Instance.SetExpUI(exp, max_exp, level == max_level);
    }

    private void SetLevelUI()
    {
        StoreUI.Instance.SetLevelUI(level);
    }

    private void SetGoldUI()
    {
        StoreUI.Instance.SetGoldUI(gold, interest);
    }
    
}