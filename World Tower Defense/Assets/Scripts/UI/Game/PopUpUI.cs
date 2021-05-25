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
    private GameObject obj_LeakGold;

    private Text text_stage;
    private Text text_score_win;
    private Text text_score_lose;

    private Slider slider_backSound;
    private Slider slider_effectSound;
    
    private POPUP_STATE selectedUI;
    public override void Init()
    {
        Instance = this;
        selectedUI = POPUP_STATE.None;

        dic_PopUp.Add(POPUP_STATE.StageStart, transform.Find("StageStartUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.GameWin, transform.Find("GameWinUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.GameLose, transform.Find("GameLoseUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.Option, transform.Find("OptionUI").gameObject);
        dic_PopUp.Add(POPUP_STATE.LackGold, transform.Find("LackGoldUI").gameObject);

        text_stage = transform.Find("StageStartUI/Text").GetComponent<Text>();
        text_score_win = transform.Find("GameWinUI/ScoreText").GetComponent<Text>();
        text_score_lose = transform.Find("GameLoseUI/ScoreText").GetComponent<Text>();

        slider_backSound = transform.Find("OptionUI/BackgroundSlider").GetComponent<Slider>();
        slider_effectSound = transform.Find("OptionUI/EffectSlider").GetComponent<Slider>();
        slider_backSound.value = PlayerPrefs.GetFloat("BackgroundSound");
        slider_effectSound.value = PlayerPrefs.GetFloat("EffectSound");
        slider_backSound.onValueChanged.AddListener(volume => 
            SoundManager.Instance.ControlVolume(SOUNDTYPE.BACKGROUND, volume));
        slider_effectSound.onValueChanged.AddListener(volume => 
            SoundManager.Instance.ControlVolume(SOUNDTYPE.EFFECT, volume));

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
            case POPUP_STATE.Option:
                TimeManager.Instance.Pause();
                break;
            case POPUP_STATE.StageStart:
                SetStageText(p);
                StartCoroutine(CloseSelf(POPUP_STATE.StageStart, 1.5f));
                break;
            case POPUP_STATE.LackGold:
                StartCoroutine(CloseSelf(POPUP_STATE.LackGold, 1.0f));
                break;
            case POPUP_STATE.GameWin:
            case POPUP_STATE.GameLose:
                SetScoreText(p);
                break;

        }
    }
    
    private IEnumerator CloseSelf(POPUP_STATE p_state, float p_time)
    {
        yield return new WaitForSeconds(p_time);
        dic_PopUp[p_state].SetActive(false);
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
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 10);
        SoundManager.Instance.PlaySound(SOUNDTYPE.BACKGROUND, 0);
        SceneManager.Instance.LoadScene("Lobby");
    }

    private void ReGame()
    {
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 10);
        SceneManager.Instance.LoadScene("Loading");
        SceneManager.Instance.LoadScene("Game");
    }

    private void Resume()
    {
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 10);
        if (selectedUI != POPUP_STATE.None)
            dic_PopUp[selectedUI].SetActive(false);
        TimeManager.Instance.StartTime();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
