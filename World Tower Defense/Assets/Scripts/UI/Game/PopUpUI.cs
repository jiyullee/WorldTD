using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviourSubUI
{
    public static PopUpUI Instance;
    Dictionary<POPUP_STATE, GameObject> dic_PopUp = new Dictionary<POPUP_STATE, GameObject>();
    private GameObject obj_StageStart;
    private GameObject obj_GameWin;
    private GameObject obj_GameLose;

    private Text text_stage;
    private Text text_score_win;
    private Text text_score_lose;

    private POPUP_STATE selectedUI;
    public override void Init()
    {
        Instance = this;
        selectedUI = POPUP_STATE.None;
        
        dic_PopUp.Add(POPUP_STATE.StageStart, transform.Find("StageStartUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.GameWin,  transform.Find("GameWinUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.GameLose, transform.Find("GameLoseUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.Option, transform.Find("OptionUI").gameObject);
        
        text_stage = transform.Find("StageStartUI/Text").GetComponent<Text>();
        text_score_win = transform.Find("GameWinUI/ScoreText").GetComponent<Text>();
        text_score_lose = transform.Find("GameLoseUI/ScoreText").GetComponent<Text>();
        
        AddButtonEvent("GameWinUI/ExitButton", LoadLobby);
        AddButtonEvent("GameWinUI/ReGameButton", ReGame);
        AddButtonEvent("GameLoseUI/ExitButton", LoadLobby);
        AddButtonEvent("GameLoseUI/ReGameButton", ReGame);
        AddButtonEvent("OptionUI/ResumeButton", Resume);
        AddButtonEvent("OptionUI/LobbyButton", LoadLobby);
        AddButtonEvent("OptionUI/ExitButton", ExitGame);
    }

    /// <summary>
    /// 스테이지 시작, 게임 승리, 게임 종료 시 UI 팝업
    /// </summary>
    /// <param name="p_state"></param>
    /// <param name="p"></param>
    public void PopUp(POPUP_STATE p_state, int p = 0)
    {
        selectedUI = p_state;
        dic_PopUp[p_state].SetActive(true);
        switch (p_state)
        {
            case POPUP_STATE.StageStart:
                SetStageText(p);
                Invoke("Close", 1.5f);
                break;
            case POPUP_STATE.GameWin:
            case POPUP_STATE.GameLose:
                SetScoreText(p);
                break;
        }
    }

    public void Close()
    {
        dic_PopUp[POPUP_STATE.StageStart].SetActive(false);
        selectedUI = POPUP_STATE.None;
    }
    
    private void SetStageText(int p_stage)
    {
        text_stage.text = $"{p_stage} 스테이지 시작";
    }
    
    private void SetScoreText(int p_score)
    {
        text_score_win.text = $"스코어 : {p_score}";
        text_score_lose.text = $"스코어 : {p_score}";
    }

    private void LoadLobby()
    {
        SceneManager.Instance.LoadScene("Lobby");
    }

    private void ReGame()
    {
       Debug.Log("ReGame"); 
    }
    
    private void Resume()
    {
        if(selectedUI != POPUP_STATE.None)
            dic_PopUp[selectedUI].SetActive(false);
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}
