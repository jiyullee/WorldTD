using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Polling : UnitySingleton<Polling>
{
    [SerializeField]
    [Tooltip("초기 객체를 얼마나 생성할지")]
    private int initiaCount = 10;
    [SerializeField]
    [Tooltip("풀링할 객체 프리팹")]
    private PollingObject[] poolingGameObject;

    private Dictionary<string, Queue<PollingObject>> pollingQueueDictionary = new Dictionary<string, Queue<PollingObject>>();
    private Dictionary<string, PollingObject> pollingObjectDictionary = new Dictionary<string, PollingObject>();

    //생성
    public override void OnCreated()
    {
        for (int j = 0; j < poolingGameObject.Length; j++)
        {
            pollingObjectDictionary.Add(poolingGameObject[j].PrefabName, poolingGameObject[j]);
            pollingQueueDictionary.Add(poolingGameObject[j].PrefabName, new Queue<PollingObject>());
        }
    }

    //초기화
    public override void OnInitiate()
    {
        foreach (var i in pollingObjectDictionary)
        {
            for (int j = 0; j < initiaCount; j++)
                pollingQueueDictionary[i.Key].Enqueue(CreateNewObject(i.Key));
        }
    }

    //새로 만드는 함수
    private PollingObject CreateNewObject(string prefabName)
    {
        var newObj = Instantiate(pollingObjectDictionary[prefabName]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(Instance.transform);
        return newObj;
    }

    //싱글톤으로 꺼내는 함수, 인수로는 꺼내는 객체와 꺼내는 프리팹 이름을 넣어야함(객체의 부모가 될것)
    public static PollingObject GetObject(GameObject callObject, string name)
    {
        PollingObject obj;
        if (Instance.pollingQueueDictionary.ContainsKey(name) == false)
        {
            Debug.Log("is not PollingObjectName");
            return null;
        }

        //없으면 생성, 있으면 큐 꺼내서 반환
        if (Instance.pollingQueueDictionary[name].Count > 0)
        {
            obj = Instance.pollingQueueDictionary[name].Dequeue();
        }
        else
        {
            obj = Instance.CreateNewObject(name);

        }
        if (obj == null)
        {
            Debug.Log("Polling.GetObject Err");
        }

        obj.transform.position = callObject.transform.position;
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(callObject.transform);
        return obj;
    }

    //싱글톤에게 반환해주는 함수 인자로는 풀링된 객체(gameobject)를 줘야함.
    public static void ReturnObject(PollingObject pollingObj)
    {
        pollingObj.gameObject.SetActive(false);
        pollingObj.transform.SetParent(Instance.transform);
        Instance.pollingQueueDictionary[pollingObj.PrefabName].Enqueue(pollingObj);
    }
}