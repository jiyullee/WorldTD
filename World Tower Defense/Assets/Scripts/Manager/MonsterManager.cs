using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterManager : UnitySingleton<MonsterManager>
{
    #region Fields

    public const string MonsterSpritePath = "Images/Monsters/spaceships";
    [SerializeField] private float spawnCycle = 0.5f;
    
    private GameObject monsterContainer;

    //현재 필드에 스폰된 몬스터 리스트
    public static List<PollingObject> spawned_monsters = new List<PollingObject>();
    private bool flag = true;

    #endregion

    public static bool IsPoolingObject(PollingObject pollingObject) => spawned_monsters.Contains(pollingObject);
    
    #region CallBacks

    public override void OnCreated()
    {
        Transform startTransfrom = LoadManager.Instance.GetMap()[0].gameObject.transform;

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

    #endregion

    #region Functions

    public void StartSpown()
    {
        StartCoroutine("SpawnMonster");
    }

    /// <summary>
    /// 몬스터를 amount개 소환하는 Spawner
    /// </summary>
    IEnumerator SpawnMonster()
    {
        int key = StageManager.Instance.Stage - 1;
        int amount = MonsterData.Instance.GetTableData(key).Amount;
        amount = LevelManager.Instance.SetamountWeight(amount);
        
        while (amount > 0)
        {
            yield return new WaitForSeconds(spawnCycle);
            Monster monster = (Monster)PoolingManager.GetObject(monsterContainer, "Monster");
            AddMonster(monster);
            monster.SetMonsterData(key);
            amount--;
        }
        while (spawned_monsters.Count > 0)
        {
            yield return new WaitForEndOfFrame();
        }
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
