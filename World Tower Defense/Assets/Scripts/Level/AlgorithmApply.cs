using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using GameData;


/// <summary>
/// 유전자를 적용하는 클래스
/// json파일을 전부 읽어서 할당한 뒤에 그중 적합한 유전자를 검출해냄
/// </summary>
public class AlgorithmApply : UnitySingleton<AlgorithmApply>
{
    Compatibility[] compatibilitys;
    private float compatibilityFit;
    private float[] MaxClearTimes;
    private int[] gens;


    /// <summary>
    /// 각각 데이터간 적합도를 검사해서 알맞게 배분함.
    /// </summary>
    public void SetGen()
    {
        for (int j = 0; j < StageManager.Instance.MaxStage; j++)
        {
            compatibilityFit = 0;
            for (int i = 0; i < compatibilitys.Length; i++)
            {
                if (compatibilityFit > compatibilitys[i].fitnessClearTimes[j])
                {
                    compatibilityFit = compatibilitys[i].fitnessClearTimes[j];
                    gens[j] = compatibilitys[i].gens[j];
                }
            }
        }
    }
    public override void OnCreated()
    {
        //json 데이터들을 불러와서 compatibilitys에 할당
        MaxClearTimes = new float[StageManager.Instance.MaxStage];
        for (int i = 0; i < StageManager.Instance.MaxStage; i++)
        {
            MaxClearTimes[i] = MonsterManager.Instance.SpawnTime + MonsterData.Instance.GetTableData(i).Speed;
        }
    }

    public override void OnInitiate()
    {
    }
}
