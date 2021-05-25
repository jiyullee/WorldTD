using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPopUpUI : MonoBehaviourSubUI
{
    private Button btn_ok;
    private Button btn_cancel;
    public override void Init()
    {
        btn_ok = transform.Find("OkButton").GetComponent<Button>();
        btn_cancel = transform.Find("CancelButton").GetComponent<Button>();

        AddButtonEvent(btn_ok, () => Application.Quit());
        AddButtonEvent(btn_cancel, () =>
        {
            if(!TimeManager.IsNull)
                TimeManager.Instance.StartTime();
            SetView(false);
        });
        SetView(false);
    }

    public override void SetView(bool state)
    {
        base.SetView(state);
        if(state && !TimeManager.IsNull)
            TimeManager.Instance.Pause();
    }
}
