using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData;
using UnityEngine;

public class TowerManager : UnitySingleton<TowerManager>
{
    public List<Tower> list_tower = new List<Tower>();
    List<Tower> list_compound = new List<Tower>();
    //타워 이름 별로 저장된 타워 자료구조
    public Dictionary<string, List<Tower>> dic_tower = new Dictionary<string, List<Tower>>();
    
    public override void OnCreated()
    {
        
    }

    public override void OnInitiate()
    {

    }

    public Tower CreateTower(TowerInstance towerInstance, Vector3 p_pos)
    {
        if (towerInstance == null) return null;

        Tower tower = (Tower)PoolingManager.GetObject(gameObject, "Tower");
        Vector2 pos = Camera.main.ScreenToWorldPoint(p_pos);
        tower.SetPosition(pos);
        tower.SetTowerData(towerInstance);
        list_tower.Add(tower);

        if (dic_tower.ContainsKey(tower.TowerName))
        {
            if (dic_tower[tower.TowerName] == null)
            {
                dic_tower[tower.TowerName] = new List<Tower>();
            }
            dic_tower[tower.TowerName].Add(tower);
        }
        else
        {
            List<Tower> list = new List<Tower>();
            list.Add(tower);
            dic_tower.Add(tower.TowerName, list);    
        }
        
        
        SynergyManager.Instance.SetSynergy();
        return tower;
    }

    public void RemoveTower(Tower p_tower)
    {
        if (dic_tower[p_tower.TowerName] != null)
            dic_tower[p_tower.TowerName].Remove(p_tower);

        list_tower.Remove(p_tower);
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

    public bool CanCompound(Tower p_tower)
    {
        List<Tower> list = dic_tower[p_tower.TowerName];
        list_compound.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].Grade == p_tower.Grade)
                list_compound.Add(list[i]);
        }

        if (list_compound.Count < 3) return false;
        else return true;
    }
    public void CompoundTower(Tower p_tower)
    {
        list_compound.Remove(p_tower);

        for (int i = 0; i < 2; i++)
        {
            RemoveTower(list_compound[i]);
            list_compound[i].ReturnTower();
        }
        
        p_tower.Upgrade();
    }

    public void SellTower(Tower p_tower)
    {
        string towerName = p_tower.TowerName;
        if (dic_tower.ContainsKey(towerName))
        {
            if (dic_tower[towerName].Contains(p_tower))
            {
                dic_tower[towerName].Remove(p_tower);
                StoreManager.Instance.SellTower(p_tower);
                p_tower.ReturnTower();
            }
        }
    }
}
