using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
public class Compatibility
{
    //아래를 통해 클리어 시간을 산출 가능
    public float[] fitnessClearTimes;
    public float[] clearTimes;
    //이를 통해 성공률을 산출 할 수 있음.
    public int clearStage = 0;
    //아래를 통해 유전자를 뽑을 수 있음.
    public int[] gens;
}

public class NewLevelManager : UnitySingleton<NewLevelManager>
{
    private Compatibility compatibility;
    public Compatibility Compatibility
    {
        get { return compatibility; }
    }
    /// <summary>
    /// 프레임 저장함
    /// </summary>
    /// <param name="time"> 현재 클리어 시간값. </param>
    public void Clear(float time)
    {
        int stage = StageManager.Instance.Stage;
        compatibility.clearTimes[stage] = time;
        clearTimesCompatibilityFit(stage);
        compatibility.clearStage++;
    }

    /// <summary>
    /// 클리어 시간 적합도 계산한다.
    /// </summary>
    public void clearTimesCompatibilityFit(int stage)
    {
        // 데이터 파싱을 통해 예상클리어 타임 얻어오기.
        float inputTimeRate = 0;
        // float inputTime = Instance.GetTableData(stage).ClaerTime;
        // 최장 클리어 타임 계산
        float maxClearTime = MonsterManager.Instance.SpawnTime + MonsterData.Instance.GetTableData(stage).Speed;
        float clearTimeRate = compatibility.clearTimes[stage] / maxClearTime;
        compatibility.fitnessClearTimes[stage] = Mathf.Abs(inputTimeRate - clearTimeRate);
    }


    public override void OnCreated()
    {
        compatibility.clearTimes = new float[StageManager.Instance.MaxStage];
    }

    public override void OnInitiate()
    { }
}
