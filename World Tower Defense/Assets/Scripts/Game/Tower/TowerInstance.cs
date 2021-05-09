using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class TowerInstance
{
    private TowerData.TowerDataClass _towerDataClass;

    public TowerInstance(){}
    public TowerInstance(int towerIndex)
    {
        _towerDataClass = TowerData.Instance.GetTableData(towerIndex);
    }

    public TowerData.TowerDataClass GetTowerData() => _towerDataClass;
}
