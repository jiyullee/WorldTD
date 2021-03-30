using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : PollingObject
{
    [SerializeField]
    protected float[] weight = { 1, 1.25f, 1.5f };
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
    private void Start()
    {
        SetDifficulty();
    }

    protected void SetDifficulty()
    {
        monsterData.HP = (int)(weight[(int)difficulty] * (float)monsterData.HP);
        monsterData.Amor = (int)(weight[(int)difficulty] * (float)monsterData.HP);
    }

    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
    }
}
