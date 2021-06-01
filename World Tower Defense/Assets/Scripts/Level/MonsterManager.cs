using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterManager : UnitySingleton<MonsterManager>
{
    #region Fields

    //군집체당 소환되는 시간
    private float spawnTime = 5f;
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
    private float weight = 1.1f;

    #endregion

    public static bool IsPoolingObject(PollingObject pollingObject) => spawned_monsters.Contains(pollingObject);

    #region CallBacks

    public override void OnCreated()
    {
        Transform startTransfrom = LoadManager.Instance.GetMap()[0].gameObject.transform;
        monsterImage = Resources.LoadAll<Sprite>("Images/Monsters/");
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


    private string SetMonster(string gen)
    {
        if (stage % 5 == 0)
        {
            nextMonster = gen;
            gen = "";
            amount = 1;
        }
        else
        {
            nextMonster = gen.Substring(0, 1);
            amount = MonsterAssocationData.Instance.GetTableData(Int32.Parse(nextMonster)).Amount;
        }
        if (gen.Length >= 1)
            return gen.Substring(1, gen.Length - 1);
        else
            return "";
    }


    public void StartSpawn()
    {
        StartCoroutine(Calculation());
        StartCoroutine(SpawnMonster());
    }

    IEnumerator Calculation()
    {
        time = 0;
        time += Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);
        while (spawned_monsters.Count > 0 || amount > 0)
        {
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// 몬스터를 amount개 소환하는 sponer
    /// </summary>
    IEnumerator SpawnMonster()
    {
        string gen = NewLevelManager.Instance.AddGen();
        stage = StageManager.Instance.Stage;
        while (gen.Length > 0)
        {
            gen = SetMonster(gen);
            spawnCycle = spawnTime / amount;
            while (amount > 0)
            {
                Monster monster = (Monster)PoolingManager.GetObject(monsterContainer, "Monster");
                monster.SetMonsterData(nextMonster, weight);
                AddMonster(monster);
                amount--;
                yield return new WaitForSeconds(spawnCycle);
                //델타 타임 생각해서 아래와 같은 코드를 짬 더 좋은 코드있으면 수정 바람
                // float watietime = 0;
                // while (spawnCycle > watietime)
                // {
                //     watietime += Time.deltaTime;
                //     yield return new WaitForSeconds(Time.deltaTime);
                // }
            }
            if (gen == "")
                break;
        }
        while (spawned_monsters.Count > 0 && monsterContainer.transform.childCount > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        if (GameManager.Instance.HP > 0)
        {
            NewLevelManager.Instance.Clear(time, stage);
            StageManager.Instance.Reward();
        }
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
