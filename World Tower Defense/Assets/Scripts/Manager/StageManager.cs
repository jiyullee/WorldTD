using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class StageManager : UnitySingleton<StageManager>
{
    [SerializeField]
    private const int MaxStage = 29;
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
        StartStage();
    }


    public void NextStage()
    {
        MonsterSponer.Instance.stage++;
        StartStage();
    }
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
    public void EndStage()
    {
        NextStage();
    }
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

