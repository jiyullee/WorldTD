using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterSponer : UnitySingleton<MonsterSponer>
{
    [SerializeField] private float stageWaitingTime = 2.0f;
    [SerializeField] private float spawnCycle = 0.5f;
    [SerializeField] private int maxStage = 30;
    [SerializeField] private Sprite[] monsterImage;
    private GameObject startPoint;
    private Sprite nextSprite;
    private Queue<PollingObject> monsterQueue;
    //현재 필드에 스폰된 몬스터 리스트
    public List<PollingObject> spawned_monsters;
    private int amount;
    private static int stage = 0;
    private bool flag = true;

    public override void OnCreated()
    {
        monsterQueue = new Queue<PollingObject>();
        startPoint = LoadManager.Instance.GetMap()[0].gameObject;
    }

    public override void OnInitiate()
    {
        
    }
    
    private void Start()
    {
        StartCoroutine("SponnerController");
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

    /// <summary>
    /// 몬스터를 스테이지 수만큼 스폰해주는 컨트롤러
    /// 다음 스테이지 갈 경우 새로 켜줘야함.
    /// </summary>
    IEnumerator SponnerController()
    {
        while (true)
        {
            if (stage >= maxStage) break;
            SetMonster();
            yield return StartCoroutine("SponMonster");
            yield return new WaitForSeconds(stageWaitingTime);
            stage++;
            //몬스터 스폰중;
        }
        //게임 클리어 코드
        Gamemanager.Instance.GameClear();
    }

    /*
        /// <summary>
        /// 몬스터의 소환 혹은 대기 상태를 결정해줌
        /// 몬스터 소환하고 다 잡을때 까지 대기할 경우 (FSM으로 사용)
        /// </summary>
        private void State()
        {
            if (monsterQueue.Count > 0 && flag == true)
            {
            }
            else if (flag == false)
            {
                StartCoroutine("SponMonster");
            }
        }
    */

    /// <summary>
    /// 몬스터를 amount개 소환하는 sponer
    /// </summary>
    IEnumerator SponMonster()
    {
        while (amount > 0)
        {
            yield return new WaitForSeconds(spawnCycle);
            Monster monster = (Monster)Polling2.GetObject(startPoint, "Monster");
            monster.SetMonsterData(stage, nextSprite);
            monsterQueue.Enqueue(monster);
            
            amount--;
        }
    }
}
