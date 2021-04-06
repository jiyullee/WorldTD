using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class TowerManager : UnitySingleton<TowerManager>
{
    private List<Tower> list_tower = new List<Tower>();
    public override void OnCreated()
    {
        
    }

    public override void OnInitiate()
    {
        
    }

    public void CreateTower(TowerInstance p_towerInstance)
    {
        Tower tower = (Tower)Polling2.GetObject(gameObject, "Tower");
        tower.SetTowerData(p_towerInstance);
        list_tower.Add(tower);
    }
    
    
}
