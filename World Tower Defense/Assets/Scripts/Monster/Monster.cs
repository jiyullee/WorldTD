using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : PollingObject
{
    protected MonsterData monsterData;
    public MonsterData MonsterData
    {
        get => monsterData;
        set => monsterData = value;
    }
    private Difficulty difficulty;
    public Difficulty Difficulty
    {
        get => difficulty;
        set => difficulty = value;
    }

    [SerializeField]
    protected float[] weight = { 1, 1.25f, 1.5f };

    protected void SetDifficulty()
    {
        monsterData.HP = (int)(weight[(int)difficulty] * (float)monsterData.HP);
        monsterData.Amor = (int)(weight[(int)difficulty] * (float)monsterData.HP);
        /*
        switch (Difficulty)
        {
            case Difficulty.easy:
                break;
            case Difficulty.nomal:
                monsterData.HP = (int)(weight[1] * (float)monsterData.HP);
                monsterData.Amor = (int)(weight[1] * (float)monsterData.HP);
                break;
            case Difficulty.hard:
                monsterData.HP = (int)(weight[2] * (float)monsterData.HP);
                monsterData.Amor = (int)(weight[2] * (float)monsterData.HP);
                break;
        }
        */
    }

    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
        SetDifficulty();
    }
}
