using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class StoreData : DataSets<StoreData, StoreData.StoreDataClass>
{
    public override void OnCreated()
    {
        fileName = ParsingDataSet.StoreData;
    }

    public class StoreDataClass : DataClass
    {
        public int Level;
        public List<float> Rarity;
        public int MaxExp;
    }
}
