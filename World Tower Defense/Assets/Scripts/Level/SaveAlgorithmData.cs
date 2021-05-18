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
    /// <summary>
    /// 꺼질때 데이터 저장.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveData();
    }
    /// <summary>
    /// 유전자를 Json으로 저장
    /// 이때 각각의 객체를 비교해주기 위해 현재시간을 string값으로 저장함
    /// 게임이 끝날때 호출
    /// </summary>
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "DataSet");
        NewLevelManager.Instance.Compatibility.Count++;
        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);
        string jsonData = JsonUtility.ToJson(NewLevelManager.Instance.Compatibility);
        string savePath = Path.Combine(saveDirectory, "Data.json");
        File.WriteAllText(savePath, jsonData);
    }

    /// <summary>
    /// Json 유전자를 받아오는 것.
    /// 시작하자 마자 실행
    /// </summary>
    [ContextMenu("Load Data")]
    public Compatibility GetData()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "DataSet");
        string savePath = Path.Combine(saveDirectory, "Data.json");
        Compatibility compatibility = new Compatibility();
        compatibility.init();
        if (Directory.Exists(saveDirectory))
        {
            string data = File.ReadAllText(savePath);
            compatibility = JsonUtility.FromJson<Compatibility>(data);
            if (compatibility.Count == compatibility.maxCount)
            {
                //유전자 조합
                AlgorithmApply.Instance.ApplyGen();
                compatibility.Count = 0;
                compatibility.isfirst = false;
            }
        }
        else
        {
            compatibility = new Compatibility();
            compatibility.init();
            compatibility.isfirst = true;
        }
        return compatibility;
    }

    public override void OnCreated()
    {
        //numberOfClusters = 군집수
    }

    public override void OnInitiate()
    {
    }
}
