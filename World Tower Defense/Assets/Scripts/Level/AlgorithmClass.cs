using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
[System.Serializable]
public class Compatibility
{
    //아래를 통해 클리어 시간을 산출 가능
    public floatArray[] clearTimeRate;
    public floatArray[] clearTimes;
    //이를 통해 성공률을 산출 할 수 있음.
    public int[] clearStages;
    //아래를 통해 유전자를 뽑을 수 있음.
    public stringArray[] gens;
    public int Count = 0;
    public int maxCount = 5;
    public int maxStage = 30;
    //첫번째인지 판별하는 플래그
    public bool isfirst = true;

    public void init()
    {
        clearTimeRate = new floatArray[maxCount];
        clearTimes = new floatArray[maxCount];
        gens = new stringArray[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
            clearTimeRate[i] = new floatArray(maxStage);
            clearTimes[i] = new floatArray(maxStage);
            gens[i] = new stringArray(maxStage);
        }
        clearStages = new int[maxCount];
        isfirst = true;
    }

}

[System.Serializable]
public class floatArray
{
    public float[] arr;
    public floatArray(int stage)
    {
        arr = new float[stage];
        for (int j = 0; j < stage; j++)
        {
            arr[j] = 1.0f;
        }
    }
}
[System.Serializable]
public class stringArray
{
    public string[] arr;
    public stringArray(int stage)
    {
        arr = new string[stage];
    }
}