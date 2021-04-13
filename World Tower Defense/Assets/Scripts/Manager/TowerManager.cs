using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class TowerManager : UnitySingleton<TowerManager>
{
    public List<Tower> list_tower = new List<Tower>();
    public override void OnCreated()
    {
        
    }

    public override void OnInitiate()
    {
        
    }

    public Tower CreateTower(TowerInstance towerInstance, Vector3 p_pos)
    {
        if (towerInstance == null) return null;

        Tower tower = (Tower)Polling2.GetObject(gameObject, "Tower");
        Vector2 pos = Camera.main.ScreenToWorldPoint(p_pos);
        tower.transform.position = new Vector3(pos.x,pos.y,0);
        tower.SetTowerData(towerInstance);
        list_tower.Add(tower);

        SynergyManager.Instance.SetSynergy();
        return tower;
    }

    public void IncreaseRange(float p)
    {
        for (int i = 0; i < list_tower.Count; i++)
        {
            list_tower[i].IncreaseRange(p);
        }
    }
    
    public void IncreaseAttack(float p)
    {
        for (int i = 0; i < list_tower.Count; i++)
        {
            list_tower[i].IncreaseAttack(p);
        }
    }
}
