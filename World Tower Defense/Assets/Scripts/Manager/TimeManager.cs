using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : UnitySingleton<TimeManager>
{
    private float tempTime = 1.0f;
    private float speed;
    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
        InitTime();
    }
    public void InitTime()
    {
        ChangeSpeed(1.0f);
        StartTime();
    }
    public void ChangeSpeed(float time)
    {
        speed = time;
        StartTime();
    }

    public void Pause()
    {
        speed = (Time.timeScale == 0) ? speed : Time.timeScale;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public void StartTime()
    {
        Time.timeScale = speed;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public void ProgressTime(float p_time, float p_max, UnityAction p_action)
    {
        StartCoroutine(Progress(p_time, p_max, p_action));
    }

    IEnumerator Progress(float p_time, float p_max, UnityAction p_action)
    {
        while (p_time > 0)
        {
            TimeUI.Instance.Progress(p_time, p_max);
            p_time -= Time.deltaTime;
            yield return null;
        }
        p_action.Invoke();
    }
}
