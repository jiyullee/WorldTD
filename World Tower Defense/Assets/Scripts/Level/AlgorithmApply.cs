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
    //유전자에 포함비율
    //위는 적합유전자 비율, 아래는 돌연변이 비율, 돌연변이는 각 mutationGenRate * 10 비율로 일어남
    private float fitnessGenRate = 0.4f;
    private float mutationGenRate = 0.3f;
    //몬스터 군집체 수
    private int monsterSize = 4;
    private int maxStage = 30;
    private float[] MaxClearTimes;
    private int originGenCount = 2;

    private Compatibility compatibility;
    public Compatibility Compatibility
    {
        get => compatibility;
        set => compatibility = value;
    }

    //입력해줘야함. 입력 받는 클리어 비율
    private float[] inputClearRate;

    private void Start()
    {

    }

    #region QuickSort
    /// <summary>
    /// 유전자와 적합도를 퀵솔트 해주는 알고리즘
    /// </summary>
    public void QuickSort(int stage, int start, int end)
    {
        if (start >= end) return;
        int pivot = Paritition(stage, start, end);
        QuickSort(stage, start, pivot - 1);
        QuickSort(stage, pivot + 1, end);

    }
    private int Paritition(int stage, int start, int end)
    {
        int pivot = start;
        int i = start + 1, j = end;
        while (i <= j)
        {

            while (compatibility.clearTimeRate[i].arr[stage] <= compatibility.clearTimeRate[pivot].arr[stage])
            {
                i++;
                if (i >= end)
                    break;
            }

            while (compatibility.clearTimeRate[j].arr[stage] >= compatibility.clearTimeRate[pivot].arr[stage])
            {
                j--;
                if (j <= start)
                    break;
            }
            if (i <= j)
                Swap(stage, i, j);
        }
        Debug.Log("pivot");
        Swap(stage, pivot, j);
        //j는 피벗이니까 냅둠
        return j;
    }

    public void Swap(int stage, int i, int j)
    {
        string tempString;
        float tempFit;

        tempString = compatibility.gens[i].arr[stage];
        compatibility.gens[i].arr[stage] = compatibility.gens[j].arr[stage];
        compatibility.gens[j].arr[stage] = tempString;
        //적합도 교환
        tempFit = compatibility.clearTimeRate[i].arr[stage];
        compatibility.clearTimeRate[i].arr[stage] = compatibility.clearTimeRate[j].arr[stage];
        compatibility.clearTimeRate[j].arr[stage] = tempFit;
    }
    #endregion


    /// <summary>
    /// 게임 클리어시 저장에서 혹은 신 로드시 count == compatibility.maxCount일 경우 호출해주는 함수
    /// 유전자를 섞어서 보존해줌
    /// </summary>
    public void ApplyGen()
    {

        //유전자 정렬
        SetGen();
        //유전자 혼합
        // MixGen();
    }

    [ContextMenu("MixGen")]
    /// <summary>
    /// 유전자 정렬
    /// </summary>
    public void SetGen()
    {
        for (int i = 1; i < maxStage; i++)
        {
            QuickSort(i, 0, compatibility.maxCount - 1);
        }
    }

    /// <summary>
    /// 유전자 섞는 알고리즘
    /// 돌연변이, 적합유전자 까지 다 한다.
    /// </summary>
    public void MixGen()
    {
        Debug.Log("mix");
        int mutantionGen = (int)Mathf.Floor(mutationGenRate * compatibility.maxCount) + 1;
        for (int j = originGenCount + 1; j < compatibility.maxCount; j++)
        {
            for (int i = 1; i < maxStage; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, originGenCount + 1);
                compatibility.gens[j].arr[i] = compatibility.gens[randomIndex].arr[i];

                // if (UnityEngine.Random.Range(0, mutantionGen) == 0)
                //     Mutent(j, i);
            }
        }
    }

    //돌연변이 유전자
    public void Mutent(int genIndex, int stage)
    {
        if (stage == 0)
            return;
        int monster = UnityEngine.Random.Range(0, monsterSize + 1);
        int randomStage = UnityEngine.Random.Range(0, stage);
        char[] tempString = new char[stage];
        if (compatibility.gens[genIndex].arr[stage] == "")
            return;
        tempString = compatibility.gens[genIndex].arr[stage].ToCharArray();
        tempString[randomStage] = monster.ToString()[0];
        compatibility.gens[genIndex].arr[stage] = tempString.ToString();
    }


    public override void OnCreated()
    {
        MaxClearTimes = new float[maxStage];
        inputClearRate = new float[maxStage];
        for (int i = 0; i < maxStage; i++)
        {
            MaxClearTimes[i] = (MonsterManager.Instance.SpawnTime * StageManager.Instance.Stage) + 24;
        }
    }

    public override void OnInitiate()
    {
    }


}
