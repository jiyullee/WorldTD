using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataSingleton : DataSets<MonsterDataSingleton.MonsterDataset>
{
    public override void OnCreated() { }

    public override void OnInitiate()
    {
        SetDictionary(ParsingDataSet.MonsterData);
    }

    public class MonsterDataset : DataClass
    {
        public int hp;
        public int amor;
        public float speed;
        public int amount;
        public string info;
    }

    public float GetSpeed(int id)
    {
        return (float)MonsterDataSingleton.Instance.GetTableData(id)["Speed"];
    }

    public float GetHP(int id)
    {
        return (float)MonsterDataSingleton.Instance.GetTableData(id)["HP"];
    }
}
