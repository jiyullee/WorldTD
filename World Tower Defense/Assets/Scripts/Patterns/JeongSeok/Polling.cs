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
    private GameObject[] poolingGameObject;
    //객체를 담을 queue
    private Queue<GameObject>[] poolingObjectQueue;
    //생성
    public override void OnCreated()
    {
        poolingObjectQueue = new Queue<GameObject>[poolingGameObject.Length];
        for (int j = 0; j < poolingGameObject.Length; j++)
        {
            poolingObjectQueue[j] = new Queue<GameObject>();
        }

    }

    //초기화
    public override void OnInitiate()
    {
        for (int j = 0; j < poolingGameObject.Length; j++)
            for (int i = 0; i < initiaCount; i++)
                poolingObjectQueue[j].Enqueue(CreateNewObject(j));

    }

    //새로 만드는 함수
    private GameObject CreateNewObject(int index)
    {
        var newObj = Instantiate(poolingGameObject[index]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(Instance.transform);
        return newObj;
    }

    //싱글톤으로 꺼내는 함수, 인수로는 꺼내는 객체를 넣어야함(객체의 부모가 될것)
    public static GameObject GetObject(GameObject callObject, int index)
    {
        GameObject obj;
        //없으면 생성, 있으면 큐 꺼내서 반환
        if (Instance.poolingObjectQueue[index].Count > 0)
        {
            obj = Instance.poolingObjectQueue[index].Dequeue();
            Debug.Log(Instance.poolingObjectQueue[index] + "Dequeue");
        }
        else
        {
            obj = Instance.CreateNewObject(index);
            Debug.Log(Instance.poolingObjectQueue[index] + "Create");

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
    public static void ReturnObject(GameObject pollingObj, int index)
    {
        pollingObj.gameObject.SetActive(false);
        pollingObj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue[index].Enqueue(pollingObj);
    }
}