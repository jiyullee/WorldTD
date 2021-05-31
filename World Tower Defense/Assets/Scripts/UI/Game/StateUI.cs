using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviourSubUI
{
    public static StateUI Instance;
    private Text text_stage;
    private Text text_towerCount;
    private Text text_hp;
    private Text text_speed;
    
    private bool IsFast;
    public override void Init()
    {
        Instance = this;
        IsFast = false;
        AddButtonEvent("OptionBtn", () =>
        {
            SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 10);
            UIManager.Instance.SetEventButton(true);
            PopUpUI.Instance.PopUp(POPUP_STATE.Option);
        });
        transform.Find("TimeUI").gameObject.AddComponent<TimeUI>().Init();

        text_hp = transform.Find("HP/HPText").GetComponent<Text>();
        text_stage = transform.Find("Stage/Text").GetComponent<Text>();
        text_towerCount = transform.Find("Tower/TowerText").GetComponent<Text>();
        text_speed = transform.Find("SpeedButton/Text").GetComponent<Text>();
        AddButtonEvent("SpeedButton", ChangeSpeed);
        SetReadyStageText();
    }

    public void SetStageText(int p_stage)
    {
        text_stage.text = p_stage + " 스테이지";
    }

    public void SetReadyStageText()
    {
        text_stage.text = "스테이지 준비";
    }
    public void SetHPText(int p_hp)
    {
        text_hp.text = $"X {p_hp}";
    }

    public void SetTowerText(int p_cnt, int p_maxCnt)
    {
        text_towerCount.text = $"{p_cnt} / {p_maxCnt}";
    }
    private void ChangeSpeed()
    {
        if (!IsFast)
        {
            TimeManager.Instance.ChangeSpeed(2);
            text_speed.text = "x2";
            IsFast = true;
        }
        else
        {
            TimeManager.Instance.ChangeSpeed(1);
            text_speed.text = "x1";
            IsFast = false;
        }
    }
}
