using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonoBehaviourSubUI : MonoBehaviour
{
    // UI 초기화 메소드
    public virtual void Init() { }
    // UI 릴리즈 메소드
    public virtual void Release() { }

    public virtual void SetView(bool state)
    {
        gameObject.SetActive(state);
    }
    
    public void AddButtonEvent(Button target, UnityAction function)
    {
        target.onClick.AddListener(() => function());
    }
    
    public void AddButtonEvent(Transform target, UnityAction function)
    {
        AddButtonEvent(target.GetComponent<Button>(), function);
    }
    
    public void AddButtonEvent(UnityAction function)
    {
        AddButtonEvent(transform.GetComponent<Button>(), function);
    }

    public void AddButtonEvent(string path, UnityAction function)
    {
        AddButtonEvent(transform.Find(path), function);
    }
}
