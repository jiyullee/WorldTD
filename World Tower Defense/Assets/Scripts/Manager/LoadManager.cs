using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : UnitySingleton<LoadManager>
{
    [SerializeField] private GameObject maps;
    private Transform[] map;

    public override void OnCreated()
    {
        if (maps == null)
        {
            Debug.Log("루트 맵을 할당해 주세요");
        }
        map = new Transform[maps.transform.childCount];
        for (int i = 0; i < maps.transform.childCount; i++)
            map[i] = maps.transform.GetChild(i);

    }
    
    /// <summary>
    /// 맵의 transform 배열을 얻을때 사용하는 함수
    /// </summary>
    public Transform[] GetMap()
    {
        if (map.Length == 0 || map[0] == null)
        {
            map = new Transform[maps.transform.childCount];
            for (int i = 0; i < maps.transform.childCount; i++)
                map[i] = maps.transform.GetChild(i);
        }
        return map;
    }

    public override void OnInitiate()
    {
    }
}
