using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class TowerData : DataSets<TowerData, TowerData.TowerDataClass>
    {
        public class TowerDataClass : DataClass
        {
            public string TowerName;
            public List<string> SynergyName;
            public int Cost;
            public List<float> Attack;
            public float Speed;
            public float Range;
        }
    }
}
