using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviourSubUI
{
    public static StateUI Instance;
    private Text text_score;
    private Text text_towerCount;
    private Text text_hp;
    
    public override void Init()
    {
        Instance = this;
        AddButtonEvent("OptionBtn", () => PopUpUI.Instance.PopUp(POPUP_STATE.Option));
        transform.Find("TimeUI").gameObject.AddComponent<TimeUI>().Init();

        text_hp = transform.Find("HP/HPText").GetComponent<Text>();
        text_score = transform.Find("Score/ScoreText").GetComponent<Text>();
        text_towerCount = transform.Find("Tower/TowerText").GetComponent<Text>();
    }

    public void SetScoreText(int p_score)
    {
        text_score.text = p_score.ToString();
    }

    public void SetHPText(int p_hp)
    {
        text_hp.text = $"X {p_hp}";
    }

    public void SetTowerText(int p_cnt, int p_maxCnt)
    {
        text_towerCount.text = $"{p_cnt}/{p_maxCnt}";
    }
}
