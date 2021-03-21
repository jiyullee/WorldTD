using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingleton
{
    /// <summary>
    /// 싱글톤 인스턴스가 생성된 경우
    /// </summary>
    void OnCreated();

    /// <summary>
    /// 초기화 메소드
    /// </summary>
    void OnInitiate();
}

public abstract class Singleton<T> : ISingleton where T : Singleton<T>, new()
{
    public abstract void OnCreated();

    public abstract void OnInitiate();

    protected static T _instance;

    public static T Instance => _instance != null ? _instance : GetInstanceObject();

    protected static T GetInstanceObject()
    {
        if (_instance != null) return _instance;

        _instance = new T();

        var _singleton = _instance as Singleton<T>;
        
        _singleton.OnCreated();
        _singleton.OnInitiate();
        
        return _instance;
    }
}

public abstract class UnitySingleton<T> : MonoBehaviour, ISingleton where T : UnitySingleton<T>
{
    public abstract void OnCreated();

    public abstract void OnInitiate();

    protected static T _instance;

    public static T Instance => _instance != null ? _instance : GetInstanceObject();

    protected static T GetInstanceObject()
    {
        if (_instance != null) return _instance;

        _instance = FindObjectOfType<T>();
        
        var _singleton = _instance as UnitySingleton<T>;
        
        _singleton.OnCreated();
        _singleton.OnInitiate();
        
        return _instance;
    }

    protected void Awake()
    {
        _instance = GetInstanceObject();
    }
}