using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviourSubUI
{
    public static StoreUI Instance;
    private Text goldText;
    private StoreTower[] storeTowers;
    public override void Init()
    {
        Instance = this;
        AddButtonEvent("Bottom/RefreshBtn", StoreManager.Instance.RefreshStore);
        AddButtonEvent("Bottom/LvUpBtn", LevelUp);
        storeTowers = transform.Find("Top/Scroll View/Viewport").GetComponentsInChildren<StoreTower>();
        for (int i = 0; i < storeTowers.Length; i++)
        {
            storeTowers[i].Init();
        }
    }

    public void Refresh(int idx, TowerInstance p_towerInstance)
    {
        storeTowers[idx].InitTower(p_towerInstance);
    }

    public void LevelUp()
    {
        
    }

    public void BuyTower()
    {
        
    }
}
