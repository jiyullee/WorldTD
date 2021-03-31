using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class MonsterSponer : MonoBehaviour
{
    [SerializeField] private float spawnCycle = 0.5f;
    [SerializeField] private Sprite[] monsterImage;
    private int amount;
    private static int stage = 0;
    [SerializeField] private int maxStage = 30;

    private void Awake()
    {
       
    }
    private void Start()
    {
        StartCoroutine("SponnerController");
    }

    private void SetMonster(int stage)
    {
        //monsterData.sprite = (sprite)monsterImage[monsterDataList[stage]["spriteIndex"]];
        amount = MonsterData.Instance.GetTableData(stage).Amount;
    }
    
    //유한 상태 기계 FSM
    IEnumerator SponnerController()
    {
        Debug.Log("StartSPonner");
        while (true)
        {
            if (stage >= maxStage) break;
            SetMonster(stage);
            stage++;
            yield return StartCoroutine("SponMonster");
            //yield return 몬스터 맵에 없는지 확인하는 함수
        }
        //게임 클리어 코드
        Gamemanager.Instance.GameClear();
    }

    IEnumerator SponMonster()
    {
        //Debug.Log(stage);
        while (amount > 0)
        {
           
            yield return new WaitForSeconds(spawnCycle);
            Monster mademonster = (Monster)Polling2.GetObject(this.gameObject, "Monster");
            // Monster mademonster = (Monster)Polling2.GetObject(Gamemanager.wayPoint[0], "monster");
            mademonster.SetMonsterData(stage);
            amount--;
        }
    }
}
