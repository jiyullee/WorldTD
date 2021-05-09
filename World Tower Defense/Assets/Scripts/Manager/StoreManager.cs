using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StoreManager : UnitySingleton<StoreManager>
{
    private List<TowerInstance>[] list_all_towers;
    
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
        int towerCount = StoreTowerData.Instance.GetTable().Count;
        for (int i = 0; i < towerCount; i++)
        {
            int towerIndex = StoreTowerData.Instance.GetTableData(i).TowerIndex;
            int cost = TowerData.Instance.GetTableData(towerIndex).Cost;
            TowerInstance towerInstance = new TowerInstance(towerIndex);
            list_all_towers[cost].Add(towerInstance);
        }
    }

    public void AddTower(Tower p_tower)
    {
        TowerInstance towerInstance = new TowerInstance();
        towerInstance = p_tower.towerInstance;
        list_all_towers[p_tower.Cost].Add(towerInstance);
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
        {
            PopUpUI.Instance.PopUp(POPUP_STATE.LackGold);
            return;
        }

        gold -= 1;
        SetGoldUI();
        
        for (int i = 0; i < max_store; i++)
        {
            int cost = SelectRand();
            TowerInstance randTower = list_all_towers[cost][Random.Range(0, list_all_towers[cost].Count)];
            StoreUI.Instance.Refresh(i, randTower);
        }
    }
    
    public void SetSelectedInstance(TowerInstance p_towerInstance)
    {
        selectedTowerInstance = p_towerInstance;
        isSelecting = true;
    }

    public bool CanBuyTower(int p_cost)
    {
        return gold >= p_cost;
    }
    
    public void BuyTower()
    {
        if (selectedTowerInstance == null) return;

        int cost = selectedTowerInstance.GetTowerData().Cost;
        
        //상점 출현 목록에서 제거
        if(list_all_towers[cost].Contains(selectedTowerInstance))
            list_all_towers[cost].Remove(selectedTowerInstance);
        
        gold -= cost;
        SetGoldUI();
    }

    public void SellTower(Tower p_tower)
    {
        AddTower(p_tower);
        gold += p_tower.Cost;
        SetGoldUI();
    }
    
    public void EarnGold(int n)
    {
        gold += n;
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
        SetLevelUI();
        SetExpUI();
    }

    public void ExpUp(int increase)
    {
        //골드 부족
        if (gold < 5)
        {
            PopUpUI.Instance.PopUp(POPUP_STATE.LackGold);
            return;
        }

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