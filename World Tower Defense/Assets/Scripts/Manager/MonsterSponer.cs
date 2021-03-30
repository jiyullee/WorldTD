using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSponer : MonoBehaviour
{
    [SerializeField] private float spawnCycle = 0.5f;
    [SerializeField] private Sprite[] monsterImage;
    private int amount;
    private MonsterDataSingleton.MonsterDataset monsterData;
    private int stage = 0;
    [SerializeField] private int maxStage = 30;
    private List<Dictionary<string, object>> monsterDataList;

    private void Awake()
    {
        monsterDataList = DataManager.Read(ParsingDataSet.MonsterData);
        maxStage = monsterDataList.Count;
    }
    private void Start()
    {
        StartCoroutine("SponnerController");
    }

    private void SetMonster(int stage)
    {
        monsterData.hp = (int)monsterDataList[stage]["HP"];
        monsterData.amor = (int)monsterDataList[stage]["Armor"];
        monsterData.speed = (int)monsterDataList[stage]["Speed"];
        monsterData.sprite = (Sprite)monsterImage[0];
        //monsterData.sprite = (sprite)monsterImage[monsterDataList[stage]["spriteIndex"]];
        amount = (int)monsterDataList[stage]["Amount"];
    }


    //유한 상태 기계 FSM
    IEnumerator SponnerController()
    {
        Debug.Log("StartSPonner");
        while (true)
        {
            SetMonster(stage++);
            yield return StartCoroutine("SponMonster");
            //yield return 몬스터 맵에 없는지 확인하는 함수
            if (stage == maxStage)
                break;
        }
        //게임 클리어 코드
        Gamemanager.Instance.GameClear();
    }

    IEnumerator SponMonster()
    {
        while (amount > 0)
        {
            yield return new WaitForSeconds(spawnCycle);
            Monster mademonster = (Monster)Polling2.GetObject(this.gameObject, "monster");
            // Monster mademonster = (Monster)Polling2.GetObject(Gamemanager.wayPoint[0], "monster");
            mademonster.MonsterData = monsterData;
            amount--;
        }
    }
}
