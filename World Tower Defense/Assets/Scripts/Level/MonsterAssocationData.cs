using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class MonsterAssocationData : DataSets<MonsterAssocationData, MonsterAssocationData.MonsterDataClass>
    {
        public override void OnCreated()
        {
            base.OnCreated();
            fileName = ParsingDataSet.MonsterAssociationData;
        }

        public class MonsterDataClass : DataClass
        {
            public int HP;
            public int Armor;
            public float Speed;
            public int Amount;
            public int spriteIndex;
        }

    }
}
