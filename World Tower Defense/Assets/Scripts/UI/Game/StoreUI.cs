using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviourSubUI
{
    private Text goldText;
    private Button[] btns_tower;
    public override void Init()
    {
        AddButtonEvent("Bottom/RefreshBtn", Refresh);
        AddButtonEvent("Bottom/LvUpBtn", LevelUp);
        btns_tower = transform.Find("Top/Scroll View/Viewport").GetComponentsInChildren<Button>();
        for (int i = 0; i < btns_tower.Length; i++)
        {
            AddButtonEvent(btns_tower[i], BuyTower);
        }
        
    }

    private void Refresh()
    {
        
    }

    private void LevelUp()
    {
        
    }

    private void BuyTower()
    {
        
    }
}
