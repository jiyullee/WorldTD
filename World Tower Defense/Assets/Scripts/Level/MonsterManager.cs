using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterManager : UnitySingleton<MonsterManager>
{
    #region Fields

    //군집체당 소환되는 시간
    [SerializeField] private float spawnTime = 5;
    public float SpawnTime { get => spawnTime; }
    private float spawnCycle;
    public Sprite[] monsterImage;
    private int amount = 0;
    private GameObject monsterContainer;
    private string nextMonster;
    //현재 필드에 스폰된 몬스터 리스트s
    public static List<PollingObject> spawned_monsters = new List<PollingObject>();
    private int stage;
    private float time;

    #endregion

    public static bool IsPoolingObject(PollingObject pollingObject) => spawned_monsters.Contains(pollingObject);

    #region CallBacks

    public override void OnCreated()
    {
        Transform startTransfrom = LoadManager.Instance.GetMap()[0].gameObject.transform;
        monsterImage = Resources.LoadAll<Sprite>("Images/Monsters/spaceships");
        // monsterContainer = Instantiate(new GameObject(), startTransfrom.position, startTransfrom.rotation); 아래 코드와 동일
        if (monsterContainer == null)
            monsterContainer = new GameObject();
        monsterContainer.transform.position = startTransfrom.transform.position;
        monsterContainer.gameObject.name = "monsterContainer";
        monsterContainer.transform.position = startTransfrom.position;
    }

    public override void OnInitiate()
    {

    }

    private void OnDestroy()
    {
        spawned_monsters.Clear();
    }

    #endregion

    #region Functions

    /// <summary>
    /// 진을 읽어서 맞는 몬스터 생성
    /// </summary>
    private string GetGen()
    {
        return NewLevelManager.Instance.AddGen();
    }

    private string SetMonster(string gen)
    {
        nextMonster = gen.Substring(0, 1);
        int nextMonsterIndex = Int32.Parse(nextMonster);
        amount = MonsterAssocationData.Instance.GetTableData(nextMonsterIndex).Amount;
        return gen.Substring(0, gen.Length - 1);
    }



    public void StartSpawn()
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
        string gen = GetGen();
        stage = StageManager.Instance.Stage;
        while (gen.Length > 0)
        {
            gen = SetMonster(gen);
            spawnCycle = spawnTime / amount;
            while (amount > 0)
            {
                Monster monster = (Monster)PoolingManager.GetObject(monsterContainer, "Monster");
                monster.SetMonsterData(nextMonster);
                AddMonster(monster);
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

    public void AddMonster(Monster p_monster)
    {
        spawned_monsters.Add(p_monster);
    }

    public void RemoveMonster(Monster p_monster)
    {
        if (spawned_monsters.Contains(p_monster))
            spawned_monsters.Remove(p_monster);
    }

    #endregion

}
