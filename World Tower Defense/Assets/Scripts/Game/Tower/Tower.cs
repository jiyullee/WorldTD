using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class Tower : PollingObject
{
    private string towerName;
    private string[] synergyName;
    private int cost;
    private int grade;
    
    private float range;
    private float speed;
    private float[] attack;

    private float cur_attack;
    public override void OnCreated()
    {
        
    }

    public override void OnInitiate()
    {
        
    }
    public void SetTowerData(TowerInstance p_towerInstance)
    {
        TowerData.TowerDataClass towerData = p_towerInstance.GetTowerData();
        towerName = towerData.TowerName;
        synergyName = towerData.SynergyName.ToArray();
        cost = towerData.Cost;
        range = towerData.Range;
        speed = towerData.Speed;
        attack = towerData.Attack.ToArray();
        grade = 1;
        cur_attack = attack[grade];
    }

   
}