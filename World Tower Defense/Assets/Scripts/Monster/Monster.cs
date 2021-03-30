using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : PollingObject
{
    [SerializeField]
    protected float[] weight = { 1, 1.25f, 1.5f };
    protected MonsterDataSingleton.MonsterDataset monsterData;
    public MonsterDataSingleton.MonsterDataset MonsterData
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
    private void Start()
    {
        SetDifficulty();
    }

    protected void SetDifficulty()
    {
        monsterData.hp *= (int)weight[(int)difficulty];
        monsterData.amor *= (int)weight[(int)difficulty];
    }

    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
    }
}
