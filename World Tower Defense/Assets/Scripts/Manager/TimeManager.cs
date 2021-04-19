using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : UnitySingleton<TimeManager>
{
    private float tempTime = 1.0f;

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
    }
    public void ChangeSpeed(float time)
    {
        Time.timeScale = time;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public void Pause()
    {
        tempTime = Time.timeScale;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public void StartTime()
    {
        Time.timeScale = tempTime;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
