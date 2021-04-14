using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISPollingObject : ISingleton
{
    /// <summary>
    /// 딕셔너리, 큐에 넣기위해 이름 생성
    /// </summary>
    void SetName();

}

public abstract class PollingObject : MonoBehaviour, ISPollingObject
{
    [SerializeField]
    protected string prefabName;

    protected List<Synergy> list_synergy;
    
    public abstract void OnCreated();
    public abstract void OnInitiate();

    public virtual void SetName()
    {
        if (prefabName == "")
            prefabName = this.gameObject.name;
    }

    public string PrefabName
    {
        get
        {
            return prefabName;
        }
    }
    private void Awake()
    {
        SetName();
        OnCreated();
        OnInitiate();
    }
    
}