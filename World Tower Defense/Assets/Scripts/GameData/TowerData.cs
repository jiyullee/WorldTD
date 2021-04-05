using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class TowerData : DataSets<TowerData, TowerData.TowerDataClass>
    {
        public class TowerDataClass
        {
            public string TowerName;
            public string SynergyName;
            public int Cost;
            public float Attack;
            public float Speed;
            public float Range;
        }
    }
}
