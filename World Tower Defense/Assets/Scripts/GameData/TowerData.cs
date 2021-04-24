using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class TowerData : DataSets<TowerData, TowerData.TowerDataClass>
    {
        public override void OnCreated()
        {
            base.OnCreated();
            fileName = ParsingDataSet.TowerData;
        }

        public class TowerDataClass : DataClass
        {
            public string TowerName;
            public List<string> SynergyName;
            public int Cost;
            public List<float> Damage;
            public float Speed;
            public float Range;
        }
        
        
    }
}
