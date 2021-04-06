using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private string towerName;
    private string[] synergyName;
    private int cost;
    private int grade;
    
    private float range;
    private float speed;
    private float[] attack;
    
    public void SetTowerData(int key)
    {
        TowerData.TowerDataClass towerDataClass = TowerData.Instance.GetTableData(key);
        towerName = towerDataClass.TowerName;
        synergyName = towerDataClass.SynergyName.ToArray();
        cost = towerDataClass.Cost;
        range = towerDataClass.Range;
        speed = towerDataClass.Speed;
        attack = towerDataClass.Attack.ToArray();
        grade = 1;
    }
}