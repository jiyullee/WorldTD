using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : UnitySingleton<DataManager>
{
    List<Dictionary<string, object>> monsterDataList = new List<Dictionary<string, object>>();

    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
        monsterDataList = CsvPaser.Read("MonsterData");
    }
}
