using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterManager : UnitySingleton<MonsterManager>
{
    //군집체당 소환되는 시간
    [SerializeField] private float spawnTime = 5;
    public float SpawnTime { get => spawnTime; }
    private float spawnCycle;
    private Sprite[] monsterImage;
    private int amount = 0;
    private GameObject monsterContainer;
    private Sprite nextSprite;
    private string nextMonster;
    private Queue<PollingObject> monsterQueue;
    //현재 필드에 스폰된 몬스터 리스트s
    public static List<PollingObject> spawned_monsters = new List<PollingObject>();
    public float stageWaitingTime { get; set; }
    private int stage;
    private float time;

    public static bool IsPoolingObject(PollingObject pollingObject) => spawned_monsters.Contains(pollingObject);

    public override void OnCreated()
    {
        Transform startTransfrom = LoadManager.Instance.GetMap()[0].gameObject.transform;
        monsterImage = Resources.LoadAll<Sprite>("Image/spaceships");
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
    /// 진을 읽어서 맞는 몬스터 생성
    /// </summary>
    private string GetGen()
    {
        string Gens = NewLevelManager.Instance.AddGen();
        return Gens;
    }

    private string SetMonster(string gen)
    {
        nextMonster = gen.Substring(gen.Length - 1);
        int nextMonsterIndex = Convert.ToInt32(nextMonster);
        nextSprite = monsterImage[MonsterAssocationData.Instance.GetTableData(nextMonsterIndex).spriteIndex];
        amount = MonsterAssocationData.Instance.GetTableData(nextMonsterIndex).Amount;
        return gen.Substring(0, gen.Length - 1);
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
        string gen = GetGen();
        stage = StageManager.Instance.Stage;
        while (gen.Length > 0)
        {
            gen = SetMonster(gen);
            spawnCycle = spawnTime / amount;
            while (amount > 0)
            {
                Monster monster = (Monster)PoolingManager.GetObject(monsterContainer, "Monster");
                monster.SetMonsterData(nextMonster, nextSprite);
                //이거 필요없어 보이네? 차후 주석 해볼것.
                monsterQueue.Enqueue(monster);
                spawned_monsters.Add(monster);
                amount--;
                yield return new WaitForSeconds(spawnCycle);
            }
        }
        while (spawned_monsters.Count > 0 && monsterContainer.transform.childCount > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        // NewLevelManager.Instance.Clear(time);
        StageManager.Instance.Reward();
    }

}
