using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviourSubUI
{
    private Text scoreText;
    private Text roundText;
    private Text heartText;
    

    public override void Init()
    {
        AddButtonEvent("OptionBtn", () => PopUpUI.Instance.PopUp(POPUP_STATE.Option));
        transform.Find("TimeUI").gameObject.AddComponent<TimeUI>().Init();
    }
    
}
