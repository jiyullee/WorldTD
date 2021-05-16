using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
[System.Serializable]
public class Compatibility
{
    //아래를 통해 클리어 시간을 산출 가능
    public float[][] clearTimeRate;
    public float[][] clearTimes;
    //이를 통해 성공률을 산출 할 수 있음.
    public int[] clearStages;
    //아래를 통해 유전자를 뽑을 수 있음.
    public string[][] gens;
    public int Count = 0;
    public int maxCount = 10;
    public int maxStage = 30;
    //첫번째인지 판별하는 플래그
    public bool isfirst = true;

    public void init()
    {
        clearTimeRate = new float[maxCount][];
        clearTimes = new float[maxCount][];
        gens = new string[maxCount][];
        for (int i = 0; i < maxCount; i++)
        {
            clearTimeRate[i] = new float[maxStage];
            clearTimes[i] = new float[maxStage];
            gens[i] = new string[maxStage];
        }
        clearStages = new int[maxStage];
        isfirst = true;
    }
}

public class NewLevelManager : UnitySingleton<NewLevelManager>
{
    private Compatibility compatibility;
    public Compatibility Compatibility
    {
        get => compatibility;
        set => compatibility = value;
    }
    //몬스터 종류
    private int numberOfClusters = 3;


    /// <summary>
    /// 다음 유전자를 랜덤으로 더해주고 저장함.
    /// 유전자들은 개별로 stage유전자를 소유하고 있음.
    /// </summary>
    /// <returns> 유전자를 리턴해줌</returns>
    public string AddGen()
    {
        int random = UnityEngine.Random.Range(0, numberOfClusters + 1);
        int stage = StageManager.Instance.Stage;
        //초기 생성 단계
        if (Compatibility.isfirst == true)
        {
            compatibility.gens[compatibility.Count][stage] = (stage == 1) ? random.ToString() : compatibility.gens[compatibility.Count][stage - 1] + random.ToString();
        }
        //섞은 이후 는 그냥 넘김
        return compatibility.gens[compatibility.Count][stage];
    }


    /// <summary>
    /// 클리어 시간 저장, 스테이지를 올려준다.
    /// </summary>
    /// <param name="time"> 현재 클리어 시간값. </param>
    public void Clear(float time)
    {
        int stage = StageManager.Instance.Stage;
        compatibility.clearTimes[compatibility.Count][stage] = time;
        compatibility.clearTimeRate[compatibility.Count][stage] = clearTimeRate(stage);
        compatibility.clearStages[compatibility.Count]++;
    }

    /// <summary>
    /// 클리어 시간 적합도 계산한다.
    /// </summary>
    // 계산식은 |입력된 시간비율 - 계산시간비율|
    // 계산 시간은 현재 클리어 시간/최장 클리어 시간 이된다.
    public float clearTimeRate(int stage)
    {
        // 데이터 파싱을 통해 예상클리어 타임 얻어오기.
        float inputTimeRate = 0;
        float inputTime = AlogrithmData.Instance.GetTableData(stage).fitnessClearTimeRate;
        // 최장 클리어 타임 계산
        float maxClearTime = (MonsterManager.Instance.SpawnTime * StageManager.Instance.Stage) + 24;
        float clearTimeRate = compatibility.clearTimes[compatibility.Count][stage] / maxClearTime;
        return Mathf.Abs(inputTimeRate - clearTimeRate);
    }



    public override void OnCreated()
    {

    }

    public override void OnInitiate()
    {
        compatibility = SaveAlgorithmData.Instance.GetData();
    }
}
