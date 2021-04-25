using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Spownclass
{
    public int count;
    public PollingObject pollingObject;
}

public class Polling2 : UnitySingleton<Polling2>
{
    [SerializeField]
    private Spownclass[] spown;

    private Dictionary<string, Queue<PollingObject>> pollingQueueDictionary = new Dictionary<string, Queue<PollingObject>>();
    private Dictionary<string, PollingObject> pollingObjectDictionary = new Dictionary<string, PollingObject>();

    //생성
    public override void OnCreated()
    {

        if (spown.Length == 0)
        {
            Debug.Log("풀링할 객체가 없습니다. 할당해 주세요");
        }
        for (int j = 0; j < spown.Length; j++)
        {
            pollingObjectDictionary.Add(spown[j].pollingObject.PrefabName, spown[j].pollingObject);
            pollingQueueDictionary.Add(spown[j].pollingObject.PrefabName, new Queue<PollingObject>());
        }
    }

    //초기화
    public override void OnInitiate()
    {

        for (int i = 0; i < spown.Length; i++)
            for (int j = 0; j < spown[i].count; j++)
                pollingQueueDictionary[spown[i].pollingObject.name].Enqueue(CreateNewObject(spown[i].pollingObject.name));
    }

    //새로 만드는 함수
    private PollingObject CreateNewObject(string prefabName)
    {
        var newObj = Instantiate(pollingObjectDictionary[prefabName]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(Instance.transform);
        return newObj;
    }

    /// <summary>
    /// 싱글톤으로 꺼내는 함수, 인수로는 꺼내는 객체와 꺼내는 프리팹 이름을 넣어야함(객체의 부모가 될것)
    /// </summary>
    public static PollingObject GetObject(GameObject callObject, string name)
    {
        if (Instance.pollingQueueDictionary.ContainsKey(name) == false)
        {
            Debug.Log("is not PollingObjectName");
            return null;
        }
        PollingObject Monster;

        //없으면 생성, 있으면 큐 꺼내서 반환
        if (Instance.pollingQueueDictionary[name].Count > 0)
        {
            Monster = Instance.pollingQueueDictionary[name].Dequeue();
        }
        else
        {
            Monster = Instance.CreateNewObject(name);

        }
        if (Monster == null)
        {
            Debug.Log("Polling.GetObject Err");
        }

        Monster.transform.position = callObject.transform.position;
        Monster.gameObject.SetActive(true);
        Monster.transform.SetParent(callObject.transform);
        return Monster;
    }

    /// <summary>
    /// 싱글톤에게 반환해주는 함수 PollingObject = this
    /// </summary>
    public static void ReturnObject(PollingObject pollingObj)
    {
        pollingObj.gameObject.SetActive(false);
        pollingObj.transform.SetParent(Instance.transform);
        pollingObj.transform.position = Instance.transform.position;
        Instance.pollingQueueDictionary[pollingObj.PrefabName].Enqueue(pollingObj);
    }
}