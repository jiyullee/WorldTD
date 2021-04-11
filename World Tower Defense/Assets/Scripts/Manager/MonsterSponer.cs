using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterSponer : UnitySingleton<MonsterSponer>
{
    [SerializeField] private float spawnCycle = 0.5f;
    [SerializeField] private Sprite[] monsterImage;
    [SerializeField] private GameObject monsterContainer;
    private Sprite nextSprite;
    private Queue<PollingObject> monsterQueue;
    //현재 필드에 스폰된 몬스터 리스트s
    public static List<PollingObject> spawned_monsters = new List<PollingObject>();
    public float stageWaitingTime { get; set; }
    private int amount;
    public int stage { get; set; }
    private bool flag = true;

    public static bool IsPoolingObject(PollingObject pollingObject) => spawned_monsters.Contains(pollingObject);

    public override void OnCreated()
    {
        Transform startTransfrom = LoadManager.Instance.GetMap()[0].gameObject.transform;

        // monsterContainer = Instantiate(new GameObject(), startTransfrom.position, startTransfrom.rotation); 아래 코드와 동일
        if (monsterContainer == null)
            monsterContainer = new GameObject();
        monsterContainer.transform.position = startTransfrom.transform.position;
        monsterContainer.gameObject.name = "monsterContainer";
        monsterQueue = new Queue<PollingObject>();
        monsterContainer.transform.position = startTransfrom.position;
    }

    public override void OnInitiate()
    {
    }


    /// <summary>
    /// 몬스터의 데이터를 세팅해줌.
    /// 이미지가 생길 경우 이미지 스크립트로 해줄것.
    /// </summary>
    private void SetMonster()
    {
        nextSprite = monsterImage[MonsterData.Instance.GetTableData(stage).spriteIndex];
        amount = MonsterData.Instance.GetTableData(stage).Amount;
    }

    public void StartSpown()
    {
        StartCoroutine("SponMonster");
    }

    /// <summary>
    /// 몬스터를 amount개 소환하는 sponer
    /// </summary>
    IEnumerator SponMonster()
    {
        SetMonster();
        while (amount > 0)
        {
            yield return new WaitForSeconds(spawnCycle);
            Monster monster = (Monster)Polling2.GetObject(monsterContainer, "Monster");
            monster.SetMonsterData(stage, nextSprite);
            monsterQueue.Enqueue(monster);
            amount--;
        }
        yield return new WaitForSeconds(stageWaitingTime);
        StageManager.Instance.EndStage();
    }

}
