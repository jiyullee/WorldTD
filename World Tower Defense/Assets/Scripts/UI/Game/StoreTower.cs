using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTower : MonoBehaviourSubUI
{
    private Tower tower;

    private Button btn_store;
    private Text text_towerName;
    private Text text_cost;
    private Image img_cost;
    public override void Init()
    {
        AddButtonEvent("Button", BuyTower);
        text_towerName = transform.Find("Info/TowerName").GetComponent<Text>();
        text_cost = transform.Find("Info/Cost/Text").GetComponent<Text>();
        
    }

    public void InitTower(Tower p_tower)
    {
       
    }

    private void BuyTower()
    {
        
    }

}
