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

    public void CreateTower(TowerInstance towerInstance, Vector3 p_pos)
    {
        if (towerInstance == null) return;
        Debug.Log("TowerManager Create Tower");
        Tower tower = (Tower)Polling2.GetObject(gameObject, "Tower");
        tower.transform.position = Camera.main.ScreenToWorldPoint(p_pos) + new Vector3(0,0,5);
        tower.SetTowerData(towerInstance);
        list_tower.Add(tower);
    }
    
    
}
