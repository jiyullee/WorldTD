using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StageManager : UnitySingleton<StageManager>
{
    [SerializeField]
    private const int MaxStage = 30;
    [SerializeField]
    private float stageWaitingTime;
    private float max_waitTime;
    private int stage;
    public override void OnCreated()
    {
        MonsterSpawner.Instance.stage = 0;
        MonsterSpawner.Instance.stageWaitingTime = this.stageWaitingTime;
        max_waitTime = stageWaitingTime;
    }

    public override void OnInitiate()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        StoreManager.Instance.RefreshStore();
        StartCoroutine(Wait());
    }


    /// <summary>
    /// 스테이지를 실행해 주는 함수
    /// Start -> End -> Next -> Start순으로 루프된다.
    /// </summary>
    public void StartStage()
    {
        stage = MonsterSpawner.Instance.stage;
        if (stage > MaxStage)
        {
            StartCoroutine("CheckGameClear");
            return;
        }
        PopUpUI.Instance.PopUp(POPUP_STATE.StageStart, stage);
        MonsterSpawner.Instance.StartSpown();
    }
    /// <summary>
    /// 다음 스테이지를 켜주는 함수
    /// </summary>
    public void NextStage()
    {
        MonsterSpawner.Instance.stage++;
        StartStage();
    }

    /// <summary>
    /// 스테이지가 끝날경우 호출함(start를 호출)
    /// </summary>
    public void EndStage()
    {
        StoreManager.Instance.EarnGold();
        StoreManager.Instance.RefreshStore();
        StartCoroutine(Wait());
    }

    /// <summary>
    /// 게임이 끝난 경우 남아있는 몹들을 계속 체크해서 끝나면 게임을 종료함.
    /// </summary>
    IEnumerator CheckGameClear()
    {
        while (true)
        {
            if (MonsterSpawner.spawned_monsters.Count == 0)
                Gamemanager.Instance.GameClear();
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Wait()
    {
        while (stageWaitingTime > 0)
        {
            TimeUI.Instance.Progress(stageWaitingTime, max_waitTime);
            stageWaitingTime -= Time.deltaTime;
            yield return null;
        }
        
        TimeUI.Instance.InitTime();
        stageWaitingTime = max_waitTime;
        NextStage();
    }
}

