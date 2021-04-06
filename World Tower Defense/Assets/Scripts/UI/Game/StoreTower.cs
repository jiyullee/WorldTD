using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTower : MonoBehaviourSubUI
{
    private TowerInstance towerInstance;
    private Text text_towerName;
    private Text text_cost;
    private Image img_cost;
    public override void Init()
    {
        AddButtonEvent("Button", BuyTower);
        text_towerName = transform.Find("Info/TowerName").GetComponent<Text>();
        text_cost = transform.Find("Info/Cost/Text").GetComponent<Text>();
        
    }

    public void InitTower(TowerInstance p_towerInstance)
    {
        towerInstance = p_towerInstance;
        text_towerName.text = towerInstance.GetTowerData().TowerName;
        text_cost.text = towerInstance.GetTowerData().Cost.ToString();
        
    }

    private void BuyTower()
    {
        //TowerManager.Instance.CreateTower(towerInstance);
        StoreManager.Instance.SetSelectedInstance(towerInstance);
        MapUI.Instance.SelectableButtons(true);
    }

}
