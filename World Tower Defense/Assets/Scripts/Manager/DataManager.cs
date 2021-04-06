using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameData;

public class DataManager : UnitySingleton<DataManager>
{

    private ParsingDataSet dataSet;
    
    //메타 문자열
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    //List이기 때문에 변수명[(int)stage][(string)Key] 로 쓰면됨. 데이터 파싱할것 있으면 enum에 추가한 뒤에 파싱할것.
    public static List<Dictionary<string, object>> Read(ParsingDataSet dataSet)
    {
        string fileName = dataSet.ToString();

        if (fileName == null)
        {
            Debug.Log("파일이 없습니다.");
        }

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(fileName) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;
        string[] header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            Dictionary<string, object> entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }

    
    
    private List<Dictionary<string, object>> monsterDataList = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> MonsterDataList { get => monsterDataList; }
    private List<Dictionary<string, object>> towerDataList = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> TowerDataList { get => monsterDataList; }


    public override void OnCreated()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    public override void OnInitiate()
    {
        // 모든 데이터를 로비에서 미리 불러온다.
        MonsterData.Instance.ReadTable();
        TowerData.Instance.ReadTable();
        StoreTowerData.Instance.ReadTable();
    }
}