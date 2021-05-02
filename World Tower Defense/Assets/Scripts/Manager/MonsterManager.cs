using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterManager : UnitySingleton<MonsterManager>
{
    [SerializeField] private float spawnTime = 10;
    public float SpawnTime { get { return spawnTime; } }
    private float spawnCycle;
    [SerializeField] private Sprite[] monsterImage;
    private GameObject monsterContainer;
    private Sprite nextSprite;
    private Queue<PollingObject> monsterQueue;
    //현재 필드에 스폰된 몬스터 리스트s
    public static List<PollingObject> spawned_monsters = new List<PollingObject>();
    public float stageWaitingTime { get; set; }
    private int amount;
    private int stage;
    private float time;

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
        amount = LevelManager.Instance.SetamountWeight(amount);
    }

    public void StartSpown()
    {
        StartCoroutine(Calculation());
        StartCoroutine(SpawnMonster());
    }
    IEnumerator Calculation()
    {
        time = 0;
        while (MonsterManager.spawned_monsters.Count > 0 || amount > 0)
        {
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

    }


    /// <summary>
    /// 몬스터를 amount개 소환하는 sponer
    /// </summary>
    IEnumerator SpawnMonster()
    {
        StageManager.IsCombatting = true;
        SetMonster();
        stage = StageManager.Instance.Stage;
        spawnCycle = spawnTime / amount;
        while (amount > 0)
        {
            Monster monster = (Monster)PoolingManager.GetObject(monsterContainer, "Monster");
            monster.SetMonsterData(stage - 1, nextSprite);
            monsterQueue.Enqueue(monster);
            amount--;
            yield return new WaitForSeconds(spawnCycle);
        }
        while (MonsterManager.spawned_monsters.Count > 0 && (monsterContainer.transform.childCount > 0))
        {
            // Debug.Log("while");
            Debug.Log(MonsterManager.spawned_monsters.Count);
            // 특정 상황에서 몬스터 count가 안없어지는 상황 발생함.
            yield return new WaitForEndOfFrame();
        }
        // NewLevelManager.Instance.Clear(time);
        StageManager.Instance.Reward();
    }

}
