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
    private int monsterSize = 5;
    private float[] MaxClearTimes;

    private Compatibility compatibility;

    //입력해줘야함. 입력 받는 클리어 비율
    private float[] inputClearRate;

    private void Start()

    }

    #region QuickSort
    /// <summary>
    /// 유전자와 적합도를 퀵솔트 해주는 알고리즘
    /// </summary>
    public void QuickSort(int stage, int left, int right)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;
        Partition(stage, left, right);
        QuickSort(stage, left, mid);
        QuickSort(stage, mid + 1, right);
    }

    public void Partition(int stage, int left, int right)
    {
        int i = left, j = right;
        while (i < j)
        {
            j--;
            i++;
            Swap(stage, i, j);
        }
    }

    public void Swap(int stage, int i, int j)
    {
        string temp;
        float tempFit;
        //유전자 교환
        temp = compatibility.gens[stage].arr[i];
        compatibility.gens[stage].arr[i] = compatibility.gens[stage].arr[i];
        compatibility.gens[stage].arr[i] = temp;
        //적합도 교환
        tempFit = compatibility.clearTimeRate[stage].arr[i];
        compatibility.clearTimeRate[stage].arr[i] = compatibility.clearTimeRate[stage].arr[i];
        compatibility.clearTimeRate[stage].arr[i] = tempFit;
    }

    //0을 최 후순위로 바꾸는 알고리즘
    public void BackZero()
    {
        floatArray[] fitnessClearTimess = compatibility.clearTimeRate;
        int countX = 0, countY = 0, countX2 = 0, countY2 = 0;
        for (int i = 0; i < compatibility.maxStage; i++)
            for (int j = 0; j < compatibility.maxCount; i++)
            {
                if (compatibility.clearTimeRate[i].arr[j] == 0)
                    fitnessClearTimess[compatibility.maxStage - countX2++].arr[compatibility.maxCount - countY2++] = compatibility.clearTimeRate[i].arr[j];
                else
                    fitnessClearTimess[countX++].arr[countY++] = compatibility.clearTimeRate[i].arr[j];
            }
        compatibility.clearTimeRate = fitnessClearTimess;
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
        MixGen();
    }

    [ContextMenu("MixGen")]
    /// <summary>
    /// 유전자 정렬
    /// </summary>
    public void SetGen()
    {
        for (int i = 0; i < compatibility.maxStage; i++)
        {
            QuickSort(i, 0, compatibility.maxCount);
        }
        BackZero();
    }

    /// <summary>
    /// 유전자 섞는 알고리즘
    /// 돌연변이, 적합유전자 까지 다 한다.
    /// </summary>
    public void MixGen()
    {
        int fit = (int)Mathf.Floor(fitnessGenRate * compatibility.maxCount);
        int mutantionGen = (int)Mathf.Floor(mutationGenRate * compatibility.maxCount);
        for (int j = fit; j < compatibility.maxCount; j++)
        {
            for (int i = 0; i < compatibility.maxStage; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, fit / 10);
                compatibility.gens[j].arr[i] = compatibility.gens[randomIndex].arr[i];
                if (compatibility.maxCount - mutantionGen > j)
                    if (UnityEngine.Random.Range(0, mutantionGen / 10) == 0)
                        Mutent(j, i);
            }
        }
    }

    //돌연변이 유전자
    public void Mutent(int genIndex, int stage)
    {
        int monster = UnityEngine.Random.Range(0, monsterSize + 1);
        int randomStage = UnityEngine.Random.Range(0, stage + 1);
        char[] c = new char[stage];
        c = compatibility.gens[genIndex].arr[stage].ToCharArray();
        c[randomStage] = monster.ToString()[0];
        compatibility.gens[genIndex].arr[stage] = c.ToString();
    }


    public override void OnCreated()
    {

    }

    public override void OnInitiate()
    {
        compatibility = NewLevelManager.Instance.Compatibility;
        MaxClearTimes = new float[compatibility.maxStage];
        inputClearRate = new float[compatibility.maxStage];
        for (int i = 0; i < compatibility.maxStage; i++)
        {
            MaxClearTimes[i] = (MonsterManager.Instance.SpawnTime * StageManager.Instance.Stage) + 24;
        }
    }


}
