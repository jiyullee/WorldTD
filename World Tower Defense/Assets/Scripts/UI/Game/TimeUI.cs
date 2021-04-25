using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviourSubUI
{
    private Image img_timeBar;
    public override void Init()
    {
        img_timeBar = transform.Find("Background/TimeBar").GetComponent<Image>();
        InitTime();
    }

    public void InitTime()
    {
        img_timeBar.fillAmount = 0;
    }
    
    public void Progress(float p_time, float p_maxTime)
    {
        img_timeBar.fillAmount = p_time / p_maxTime;
    }
}
