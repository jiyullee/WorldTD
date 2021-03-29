using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSponer : MonoBehaviour
{
    [SerializeField]
    private float spawnCycle = 0.5f;
    [SerializeField]
    private Image monsterImage;
    private int amount;
    private MonsterData monsterData;
    private int stage = 0;
    private int maxStage = 30;
    List<Dictionary<string, object>> monsterDataList;

    private void Start()
    {
        SetMonster(0);
        StartCoroutine("SponMonster");
        monsterDataList = DataManager.Instance.MonsterDataList;
        maxStage = monsterDataList.Count;
    }

    void SetMonster(int stage)
    {
        monsterData = new MonsterData();
        monsterData.HP = (int)monsterDataList[stage]["HP"];
        monsterData.Amor = (int)monsterDataList[stage]["Armor"];
        monsterData.speed = (int)monsterDataList[stage]["Speed"];
        //monsterData.Image = (Image)monsterImage[monsterDataList[stage]["ImageIndex"]];
        amount = (int)monsterDataList[stage]["Amount"];
    }


    //유한 상태 기계 FSM
    IEnumerator SponNer()
    {
        while (true)
        {
            SetMonster(stage++);
            yield return StartCoroutine("SponMonster");
            if (stage == maxStage)
                break;
        }
        //게임 클리어
    }

    IEnumerator SponMonster()
    {
        while (amount > 0)
        {
            yield return new WaitForSeconds(spawnCycle);
            Monster mademonster = (Monster)Polling2.GetObject(this.gameObject, "monster");
            // Monster mademonster = (Monster)Polling2.GetObject(Gamemanager.startPoint, "monster");
            mademonster.MonsterData = monsterData;
            amount--;
        }
    }
}
