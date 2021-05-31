using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class NewLevelManager : UnitySingleton<NewLevelManager>
{

    [SerializeField] private Compatibility compatibility;
    public Compatibility Compatibility
    {
        get => compatibility;
        set => compatibility = value;
    }
    //몬스터 종류
    private int clustersSize = 3;
    private int bossCount = 1;
    private string beforeGen = "";


    /// <summary>
    /// 다음 유전자를 랜덤으로 더해주고 저장함.
    /// 유전자들은 개별로 stage유전자를 소유하고 있음.
    /// </summary>
    /// <returns> 유전자를 리턴해줌</returns>
    public string AddGen()
    {
        Debug.Log("add");
        int random = UnityEngine.Random.Range(0, clustersSize + 1);
        int stage = StageManager.Instance.Stage;
        if (stage % 5 == 0)
        {
            compatibility.gens[compatibility.Count].arr[stage] = (clustersSize + bossCount++).ToString();
        }
        //초기 생성 단계
        else if (string.IsNullOrEmpty(compatibility.gens[compatibility.Count].arr[stage]))
        {
            Debug.Log("없어");
            compatibility.gens[compatibility.Count].arr[stage] = (stage == 1) ? random.ToString() : compatibility.gens[compatibility.Count].arr[stage - 1] + random.ToString();
        }
        //섞은 이후 는 그냥 넘김
        return compatibility.gens[compatibility.Count].arr[stage];
    }


    /// <summary>
    /// 클리어 시간 저장, 스테이지를 올려준다.
    /// </summary>
    /// <param name="time"> 현재 클리어 시간값. </param>
    public void Clear(float time, int stage)
    {
        compatibility.clearTimes[compatibility.Count].arr[stage] = time;
        compatibility.clearTimeRate[compatibility.Count].arr[stage] = clearTimeRate(stage);
        compatibility.clearStages[compatibility.Count] = StageManager.Instance.Stage;
    }

    /// <summary>
    /// 클리어 시간 적합도 계산한다.
    /// </summary>
    // 계산식은 |입력된 시간비율 - 계산시간비율|
    // 계산 시간은 현재 클리어 시간/최장 클리어 시간 이된다.
    public float clearTimeRate(int stage)
    {
        // 데이터 파싱을 통해 예상클리어 타임 얻어오기.
        float maxClearTime;

        if (stage > 1 && compatibility.gens[compatibility.Count].arr[stage].Substring(0, 1) == "2")
        {
            maxClearTime = MonsterManager.Instance.SpawnTime * (stage - 1) + 30;
        }
        else
            maxClearTime = MonsterManager.Instance.SpawnTime * (stage) + 30;

        if (stage == 1 && compatibility.gens[compatibility.Count].arr[stage].Substring(0, 1) == "2")
            maxClearTime = MonsterManager.Instance.SpawnTime + 15;

        float fitness = Mathf.Abs(AlogrithmData.Instance.GetTableData(stage).fitnessClearTimeRate - compatibility.clearTimes[compatibility.Count].arr[stage] / maxClearTime);
        fitness = (float)System.Math.Round(fitness, 4);
        return fitness;
    }



    public override void OnCreated()
    {

    }

    public override void OnInitiate()
    {
        Compatibility = SaveAlgorithmData.Instance.GetData();
        AlgorithmApply.Instance.Compatibility = compatibility;
    }
}
