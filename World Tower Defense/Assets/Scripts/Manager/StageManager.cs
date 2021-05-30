using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StageManager : UnitySingleton<StageManager>
{
    #region Fields

    [SerializeField] private float stageWaitingTime;
    private float maxWaitingTime;
    [SerializeField]
    private int maxStage = 30;
    private float time = 0;
    public int MaxStage
    {
        get { return maxStage; }
    }

    private int stage;
    public int Stage => stage;
    public static bool IsCombatting;

    #endregion

    #region CallBacks

    public override void OnCreated()
    {

    }

    public override void OnInitiate()
    {
        stage = 1;
        maxWaitingTime = stageWaitingTime;
    }

    private void Start()
    {
        StoreManager.Instance.EarnGold(500);
        ReadyStage();
    }

    #endregion

    private void Update()
    {
        time += Time.deltaTime;
    }

    #region Functions

    /// <summary>
    /// 스테이지 준비 단계
    /// </summary>
    public void ReadyStage()
    {
        StoreManager.Instance.RefreshStore(false);
        TimeManager.Instance.ProgressTime(stageWaitingTime, maxWaitingTime, () =>
        {
            TimeUI.Instance.InitTime();
            stageWaitingTime = maxWaitingTime;
            NextStage();
        });
    }

    /// <summary>
    /// 스테이지를 실행해 주는 함수
    /// Start -> End -> Next -> Start순으로 루프된다.
    /// </summary>
    public void StartStage()
    {
        time = 0;
        if (stage > maxStage)
        {
            StartCoroutine("CheckGameClear");
            return;
        }
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 7);
        PopUpUI.Instance.PopUp(POPUP_STATE.StageStart, stage);
        IsCombatting = true;
        MonsterManager.Instance.StartSpawn();
    }

    /// <summary>
    /// 다음 스테이지를 켜주는 함수
    /// </summary>
    public void NextStage()
    {
        StartStage();
        stage++;
    }

    /// <summary>
    /// 스테이지 보상 주기
    /// </summary>
    public void Reward()
    {
        IsCombatting = false;
        StoreManager.Instance.EarnGold(5);
        StoreManager.Instance.ExpUp(2, false);
        ReadyStage();
    }

    /// <summary>
    /// 게임이 끝난 경우 남아있는 몹들을 계속 체크해서 끝나면 게임을 종료함.
    /// </summary>
    IEnumerator CheckGameClear()
    {
        while (true)
        {
            if (MonsterManager.spawned_monsters.Count == 0)
                GameManager.Instance.GameClear();
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}

