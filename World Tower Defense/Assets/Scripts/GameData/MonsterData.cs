using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class MonsterData : DataSets<MonsterData, MonsterData.MonsterDataClass>
    {
        public class MonsterDataClass : DataClass
        {
            public int HP;
            public int Armor;
            public float Speed;
            public int Amount;
            public string info;
            public int spriteIndex;
        }
        
    }
}
