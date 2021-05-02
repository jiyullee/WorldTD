using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


/// <summary>
/// 유전자들을 저장하는 클래스
/// NewLevelManager에서 유전자를 만들어서 이 클래스를 이용해 저장한다.
/// </summary>
public class SaveAlgorithmData : UnitySingleton<SaveAlgorithmData>
{
    //몬스터 종류
    private int numberOfClusters;
    private string saveDirectory;
    private int[] gens;

    /// <summary>
    /// 다음 유전자를 랜덤으로 더해주고 저장함.
    /// 유전자들은 개별로 stage유전자를 소유하고 있음.
    /// </summary>
    /// <returns> 다음 유전자의 인덱스, 이값을 활용해서 호출</returns>
    public int addGen()
    {
        int random = UnityEngine.Random.Range(0, numberOfClusters + 1);
        gens[StageManager.Instance.Stage] = gens[StageManager.Instance.Stage] * 10 + random;
        return random;
    }

    /// <summary>
    /// 유전자를 Json으로 저장
    /// 이때 각각의 객체를 비교해주기 위해 현재시간을 string값으로 저장함
    /// 게임이 끝날때 호출
    /// </summary>
    public void SaveGen()
    {
        Compatibility compatibility = NewLevelManager.Instance.Compatibility;
        compatibility.gens = gens;
        string jsonData = JsonUtility.ToJson(compatibility);
        string savePath = Path.Combine(saveDirectory, DateTime.Now.ToString() + "Data.json");
        File.WriteAllText(savePath, jsonData);
    }

    public override void OnCreated()
    {
        saveDirectory = Path.Combine(Application.persistentDataPath, "DataSet");
        gens = new int[StageManager.Instance.MaxStage];
        //numberOfClusters = 군집수
    }

    public override void OnInitiate()
    {
    }
}
