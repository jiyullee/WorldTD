using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : UnitySingleton<DataManager>
{
    private List<Dictionary<string, object>> monsterDataList = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> MonsterDataList { get => monsterDataList; }
    private List<Dictionary<string, object>> towerDataList = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> TowerDataList { get => monsterDataList; }

    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
        monsterDataList = CsvPaser.Read("MonsterData");
        // monsterDataList = CsvPaser.Read("TowerData");
    }
}
