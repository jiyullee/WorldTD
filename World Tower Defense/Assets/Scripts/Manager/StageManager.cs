using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StageManager : UnitySingleton<StageManager>
{
    [SerializeField]
    private int maxStage = 30;
    public int MaxStage
    {
        get { return maxStage; }
    }
    [SerializeField]
    private float stageWaitingTime;
    private float max_waitTime;
    private int stage;
    public int Stage
    {
        get { return stage; }
    }
    public static bool IsCombatting;
    public override void OnCreated()
    {
        stage = 0;
        MonsterManager.Instance.stageWaitingTime = this.stageWaitingTime;
        max_waitTime = stageWaitingTime;
    }

    public override void OnInitiate()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        StoreManager.Instance.RefreshStore();
        TimeManager.Instance.ProgressTime(stageWaitingTime, max_waitTime, () =>
        {
            TimeUI.Instance.InitTime();
            stageWaitingTime = max_waitTime;
            NextStage();
        });
    }


    /// <summary>
    /// 스테이지를 실행해 주는 함수
    /// Start -> End -> Next -> Start순으로 루프된다.
    /// </summary>
    public void StartStage()
    {
        if (stage > maxStage)
        {
            StartCoroutine("CheckGameClear");
            return;
        }
        PopUpUI.Instance.PopUp(POPUP_STATE.StageStart, stage);
        MonsterManager.Instance.StartSpown();
    }
    /// <summary>
    /// 다음 스테이지를 켜주는 함수
    /// </summary>
    public void NextStage()
    {
        stage++;
        StartStage();
    }


    /// <summary>
    /// 스테이지 보상 주기
    /// </summary>
    public void Reward()
    {
        IsCombatting = false;
        StoreManager.Instance.EarnGold();
        StoreManager.Instance.RefreshStore();
        TimeManager.Instance.ProgressTime(stageWaitingTime, max_waitTime, () =>
        {
            TimeUI.Instance.InitTime();
            stageWaitingTime = max_waitTime;
            NextStage();
        });
    }
    /// <summary>
    /// 게임이 끝난 경우 남아있는 몹들을 계속 체크해서 끝나면 게임을 종료함.
    /// </summary>
    IEnumerator CheckGameClear()
    {
        while (true)
        {
            if (MonsterManager.spawned_monsters.Count == 0)
                Gamemanager.Instance.GameClear();
            yield return new WaitForEndOfFrame();
        }
    }

}

