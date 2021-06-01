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
    public string saveDirectory;
    public string dataName = "Data.json";
    public string filePath;
    /// <summary>
    /// 세이브 혹은 로드시에 갱신됨.
    /// </summary>
    public bool isEndCollection = false;

    /// <summary>
    /// 유전자를 Json으로 저장
    /// 이때 각각의 객체를 비교해주기 위해 현재시간을 string값으로 저장함
    /// 게임을 클리어 하거나 게임이 죽었을 경우 호출
    /// </summary>
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Compatibility compatibility = NewLevelManager.Instance.Compatibility;
        compatibility.Count++;
        if (compatibility.Count == compatibility.maxCount)
            isEndCollection = true;
        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);
        string jsonData = JsonUtility.ToJson(compatibility);
        File.WriteAllText(filePath, jsonData);
    }

    /// <summary>
    /// Json 유전자를 받아오는 것.
    /// 시작하자 마자 실행
    /// </summary>
    [ContextMenu("Load Data")]
    public Compatibility GetData()
    {
        Compatibility compatibility = null;
        //아래 fileInfo가 안드에서 되는지 확인해야함
        if (new FileInfo(filePath).Exists)
        {
            string data = File.ReadAllText(filePath);
            compatibility = JsonUtility.FromJson<Compatibility>(data);
            AlgorithmApply.Instance.Compatibility = compatibility;
            if (compatibility.Count == compatibility.maxCount)
            {
                AlgorithmApply.Instance.ApplyGen();
                compatibility.Count = 0;
            }
            compatibility = AlgorithmApply.Instance.Compatibility;
        }
        if (compatibility == null)
        {
            compatibility = new Compatibility();
            compatibility.init();
        }
        return compatibility;
    }

    public override void OnCreated()
    {
        saveDirectory = Path.Combine(Application.persistentDataPath, "DataSet");
        filePath = Path.Combine(saveDirectory, dataName);
    }

    public override void OnInitiate()
    {
    }
}
