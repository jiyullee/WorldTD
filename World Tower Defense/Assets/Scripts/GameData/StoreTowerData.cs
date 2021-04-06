using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class StoreTowerData : DataSets<StoreTowerData, StoreTowerData.StoreTowerDataClass>
{
    public override void OnCreated()
    {
        base.OnCreated();
        fileName = ParsingDataSet.StoreTowerData;
    }
    public class StoreTowerDataClass : DataClass
    {
        public int TowerIndex;
        public int Count;
    }
}
