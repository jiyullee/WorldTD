using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StageManager : UnitySingleton<StageManager>
{
    [SerializeField]
    private const int MaxStage = 29;
    [SerializeField]
    private float stageWaitingTime = 3.0f;
    private int stage;
    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        MonsterSponer.Instance.stage = 0;
        MonsterSponer.Instance.stageWaitingTime = this.stageWaitingTime;
        StartStage();
    }


    /// <summary>
    /// 스테이지를 실행해 주는 함수
    /// Start -> End -> Next -> Start순으로 루프된다.
    /// </summary>
    public void StartStage()
    {
        stage = MonsterSponer.Instance.stage;
        if (stage > MaxStage)
        {
            StartCoroutine("CheckGameClear");
            return;
        }
        MonsterSponer.Instance.StartSpown();
    }
    /// <summary>
    /// 다음 스테이지를 켜주는 함수
    /// </summary>
    public void NextStage()
    {
        MonsterSponer.Instance.stage++;
        StartStage();
    }

    /// <summary>
    /// 스테이지가 끝날경우 호출함(start를 호출)
    /// </summary>
    public void EndStage()
    {
        NextStage();
    }

    /// <summary>
    /// 게임이 끝난 경우 남아있는 몹들을 계속 체크해서 끝나면 게임을 종료함.
    /// </summary>
    IEnumerator CheckGameClear()
    {
        while (true)
        {
            if (MonsterSponer.spawned_monsters.Count == 0)
                Gamemanager.Instance.GameClear();
            yield return new WaitForEndOfFrame();
        }
    }
}

