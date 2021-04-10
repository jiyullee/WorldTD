using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class SynergyData : DataSets<SynergyData, SynergyData.SynergyDataClass>
{
    public override void OnCreated()
    {
        fileName = ParsingDataSet.SynergyData;
    }

    public class SynergyDataClass : DataClass
    {
        public string SynergyName;
        public List<string> TowerList;
        public List<float> Increase;
        public List<int> ActivateNum;
    }
}
